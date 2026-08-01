using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.ReceiveAndStockPurchasedProducts;
using Application.UseCases.Auth;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Entities.Purchasing;
using Domain.Enums;
using Domain.Primitives;
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
        private readonly ICurrentUserContext _currentUserContext;

        public ReceiveAndStockPurchasedProductsCommandHandler(IIdentityService identityService, IPurchaseCommandRepository purchaseCommand, IPurchaseItemCommandRepository purchaseItemCommand, IWarehouseBatchCommandRepository warehouseBatchCommand, IMapper mapper, IUnitOfWork unitOfWork, IStockMovementCommandRepository stockMovementCommandRepository, ILogger<OnboardBrandOwnerCommandHandler> logger, ICurrentUserContext currentUserContext)
        {
            _identityService = identityService;
            _purchaseCommand = purchaseCommand;
            _purchaseItemCommand = purchaseItemCommand;
            _warehouseBatchCommand = warehouseBatchCommand;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _stockMovementCommandRepository = stockMovementCommandRepository;
            _logger = logger;
            _currentUserContext = currentUserContext;
        }

        public async Task<bool> Handle(
            ReceiveAndStockPurchasedProductsCommand request,
            CancellationToken cancellationToken)
        {
            var brandId = _currentUserContext.ActiveBrandId;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var purchase = new Purchase(new BrandId(brandId), new SupplierId(request.SupplierId));

                foreach (var itemDto in request.Items)
                {
                    var purchaseItem = new PurchaseItem(
                        purchase.Id,
                        new ProductId(itemDto.ProductId),
                        itemDto.TotalQuantity,
                        itemDto.UnitCost);

                    foreach (var batchDto in itemDto.Batches)
                    {
                        var batch = new Batch(
                            new ProductId(itemDto.ProductId),
                            purchaseItem.Id,
                            new BrandId(brandId),
                            batchDto.Quantity,
                            batchDto.UnitCost);

                        var totalDistributed = batchDto.Warehouses.Sum(w => w.Quantity);

                        if (totalDistributed != batchDto.Quantity)
                            throw new BusinessException("Warehouse distribution mismatch");

                        foreach (var warehouseDto in batchDto.Warehouses)
                        {
                            batch.DistributeToWarehouse(
                                new WarehouseId(warehouseDto.WarehouseId),
                                warehouseDto.Quantity);

                            // ✅ Stock Movement (هنا السحر الحقيقي)
                            var movement = new StockMovement(
                                new ProductId(itemDto.ProductId),
                                batch.Id,
                                new WarehouseId(warehouseDto.WarehouseId),
                                new BrandId(brandId),
                                warehouseDto.Quantity,
                                StockMovementType.PurchaseIn,
                                StockReferenceType.Purchase,
                                purchase.Id.Value);

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