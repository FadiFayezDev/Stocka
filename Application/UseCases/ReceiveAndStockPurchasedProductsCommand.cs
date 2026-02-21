using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.ReceiveAndStockPurchasedProducts;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Entities.Purchasing;
using Domain.Enums;
using Domain.Repositories.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases
{
    public class ReceiveAndStockPurchasedProductsCommand : IRequest<bool>
    {
        public Guid PurchaseId { get; init; }
        public Guid BrandId { get; init; }
        public Guid SupplierId { get; init; }

        public List<ItemReceiptDto> Items { get; init; } = new();
    }

    public class ReceiveAndStockPurchasedProductsCommandHandler : IRequestHandler<ReceiveAndStockPurchasedProductsCommand, bool>
    {
        private readonly IIdentityService _identityService;
        private readonly IPurchaseCommandRepository _purchaseCommand;
        private readonly IPurchaseItemCommandRepository _purchaseItemCommand;
        private readonly IWarehouseBatchCommandRepository _warehouseBatchCommand;

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStockMovementCommandRepository _stockMovementCommandRepository;
        private readonly ILogger<OnboardBrandOwnerCommandHandler> _logger;

        public ReceiveAndStockPurchasedProductsCommandHandler(IIdentityService identityService, IPurchaseCommandRepository purchaseCommand, IPurchaseItemCommandRepository purchaseItemCommand, IWarehouseBatchCommandRepository warehouseBatchCommand, IMapper mapper, IUnitOfWork unitOfWork, IStockMovementCommandRepository stockMovementCommandRepository, ILogger<OnboardBrandOwnerCommandHandler> logger)
        {
            _identityService = identityService;
            _purchaseCommand = purchaseCommand;
            _purchaseItemCommand = purchaseItemCommand;
            _warehouseBatchCommand = warehouseBatchCommand;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _stockMovementCommandRepository = stockMovementCommandRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(
            ReceiveAndStockPurchasedProductsCommand request,
            CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var purchase = new Purchase(request.BrandId, request.SupplierId);

                foreach (var itemDto in request.Items)
                {
                    var purchaseItem = new PurchaseItem(
                        purchase.Id,
                        itemDto.ProductId,
                        itemDto.TotalQuantity,
                        itemDto.UnitCost);

                    foreach (var batchDto in itemDto.Batches)
                    {
                        var batch = new Batch(
                            itemDto.ProductId,
                            purchaseItem.Id,
                            request.BrandId,
                            batchDto.Quantity,
                            batchDto.UnitCost);

                        var totalDistributed = batchDto.Warehouses.Sum(w => w.Quantity);

                        if (totalDistributed != batchDto.Quantity)
                            throw new BusinessException("Warehouse distribution mismatch");

                        foreach (var warehouseDto in batchDto.Warehouses)
                        {
                            batch.DistributeToWarehouse(
                                warehouseDto.WarehouseId,
                                warehouseDto.Quantity);

                            // ✅ Stock Movement (هنا السحر الحقيقي)
                            var movement = new StockMovement(
                                itemDto.ProductId,
                                batch.Id,
                                warehouseDto.WarehouseId,
                                request.BrandId,
                                warehouseDto.Quantity,
                                StockMovementType.PurchaseIn,
                                StockReferenceType.Purchase,
                                purchase.Id);

                            await _stockMovementCommandRepository.CreateAsync(movement);
                        }

                        purchaseItem.AddBatch(batch);
                    }

                    purchase.AddPurchaseItem(purchaseItem);
                }

                await _purchaseCommand.CreateAsync(purchase);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();

                _logger.LogError(ex, "Receiving & Stocking failed");

                throw;
            }
        }
    }
}