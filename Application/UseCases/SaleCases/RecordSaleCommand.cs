using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Entities.Orders;
using Domain.Entities.Products;
using Domain.Enums;
using Domain.Primitives;
using Domain.Repositories.Commands;
using MediatR;
using System.Collections.Generic;

namespace Application.UseCases.SaleCases
{
    public class RecordSaleCommand : IRequest<Response<SaleResultDto>>
    {
        public Guid EmployeeId { get; set; }
        public Guid? CustomerId { get; set; }
        public string? Notes { get; set; }
        public List<SaleItemDto> Items { get; set; } = new();
    }

    public class SaleItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class SaleResultDto
    {
        public Guid OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalProfit { get; set; }
        public List<SaleItemResultDto> Items { get; set; } = new();
    }

    public class SaleItemResultDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Cost { get; set; }
        public decimal Profit { get; set; }
    }

    public class RecordSaleCommandHandler : IRequestHandler<RecordSaleCommand, Response<SaleResultDto>>
    {
        private readonly IOrderCommandRepository _orderRepo;
        private readonly ICurrentUserContext _currentUser;
        private readonly IWarehouseCommandRepository _warehouseCommand;
        private readonly IWarehouseBatchQueryRepository _warehouseBatchQuery;
        private readonly IStockMovementCommandRepository _stockMovementRepo;
        private readonly IBatchCommandRepository _batchRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ResponseHandler _responseHandler;

        public RecordSaleCommandHandler(
            IOrderCommandRepository orderRepo,
            ICurrentUserContext currentUser,
            IWarehouseCommandRepository warehouseCommand,
            IWarehouseBatchQueryRepository warehouseBatchQuery,
            IStockMovementCommandRepository stockMovementRepo,
            IBatchCommandRepository batchRepo,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _orderRepo = orderRepo;
            _currentUser = currentUser;
            _warehouseCommand = warehouseCommand;
            _warehouseBatchQuery = warehouseBatchQuery;
            _stockMovementRepo = stockMovementRepo;
            _batchRepo = batchRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _responseHandler = new ResponseHandler();
        }

        public async Task<Response<SaleResultDto>> Handle(RecordSaleCommand request, CancellationToken cancellationToken)
        {
            if (!request.Items.Any())
                return _responseHandler.BadRequest<SaleResultDto>("Sale must have at least one item");

            var brandId = _currentUser.ActiveBrandId;
            var branchId = _currentUser.ActiveBranchId;

            if (brandId == Guid.Empty)
                return _responseHandler.Unauthorized<SaleResultDto>();

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var order = new Order(
                    new BrandId(brandId),
                    new EmployeeId(request.EmployeeId),
                    request.CustomerId.HasValue ? new CustomerId(request.CustomerId.Value) : null,
                    DateTime.UtcNow,
                    branchId.HasValue ? new BranchId(branchId.Value) : null);
                
                decimal totalAmount = 0;
                decimal totalCost = 0;
                var resultItems = new List<SaleItemResultDto>();

                foreach (var itemDto in request.Items)
                {
                    if (itemDto.Quantity <= 0)
                        return _responseHandler.BadRequest<SaleResultDto>($"Quantity must be positive for product {itemDto.ProductId}");

                    if (itemDto.UnitPrice <= 0)
                        return _responseHandler.BadRequest<SaleResultDto>($"Unit price must be positive for product {itemDto.ProductId}");

                    var pickedBatches = await PickBatchesForSale(itemDto.ProductId, itemDto.Quantity, brandId);

                    if (pickedBatches.Sum(b => b.PickedQuantity) < itemDto.Quantity)
                        return _responseHandler.BadRequest<SaleResultDto>($"Insufficient stock for product {itemDto.ProductId}");

                    decimal itemCost = 0;
                    foreach (var picked in pickedBatches)
                    {
                        var batch = await _batchRepo.GetByIdAsync(picked.BatchId.Value);
                        if (batch == null) continue;

                        batch.DeductQuantity(picked.PickedQuantity);
                        await _batchRepo.UpdateAsync(batch);

                        var warehouse = await _warehouseCommand.GetByIdAsync(picked.WarehouseId.Value);
                        if (warehouse != null)
                        {
                            var warehouseBatch = warehouse.WarehouseBatches.FirstOrDefault(wb => wb.BatchId == picked.BatchId);
                            if (warehouseBatch != null)
                                warehouse.UpdateBatchQuantity(warehouseBatch.Id, warehouseBatch.Quantity - picked.PickedQuantity);
                        }

                        var movement = new StockMovement(
                            new ProductId(itemDto.ProductId),
                            picked.BatchId,
                            picked.WarehouseId,
                            new BrandId(brandId),
                            picked.PickedQuantity,
                            StockMovementType.SaleOut,
                            StockReferenceType.Order,
                            order.Id.Value);

                        await _stockMovementRepo.CreateAsync(movement);

                        itemCost += picked.PickedQuantity * batch.UnitCost;

                        order.AddOrderItem(
                            new ProductId(itemDto.ProductId),
                            picked.BatchId,
                            picked.PickedQuantity,
                            itemDto.UnitPrice,
                            batch.UnitCost);
                    }

                    var itemTotal = itemDto.Quantity * itemDto.UnitPrice;
                    totalAmount += itemTotal;
                    totalCost += itemCost;

                    resultItems.Add(new SaleItemResultDto
                    {
                        ProductId = itemDto.ProductId,
                        Quantity = itemDto.Quantity,
                        UnitPrice = itemDto.UnitPrice,
                        Cost = itemCost,
                        Profit = itemTotal - itemCost
                    });
                }

                await _orderRepo.CreateAsync(order);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync();

                var result = new SaleResultDto
                {
                    OrderId = order.Id.Value,
                    TotalAmount = totalAmount,
                    TotalCost = totalCost,
                    TotalProfit = totalAmount - totalCost,
                    Items = resultItems
                };

                return _responseHandler.Success(result, "Sale completed successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return _responseHandler.BadRequest<SaleResultDto>($"Sale failed: {ex.Message}");
            }
        }

        private async Task<List<PickedBatch>> PickBatchesForSale(Guid productId, int quantity, Guid brandId)
        {
            var availableBatches = await _batchRepo.GetAvailableBatchesForProductAsync(productId, brandId);
            
            var sortedBatches = availableBatches
                .Where(b => b.RemainingQuantity > 0)
                .OrderBy(b => b.CreatedAt)
                .ToList();

            var warehouseBatches = (await _warehouseBatchQuery.GetAllByBrandIdAsync(brandId))
                .Where(wb => wb.Quantity > 0)
                .ToList();

            var pickedBatches = new List<PickedBatch>();
            int remainingToPick = quantity;

            foreach (var batch in sortedBatches)
            {
                if (remainingToPick <= 0) break;

                var batchesForProduct = warehouseBatches.Where(wb => wb.BatchId == batch.Id.Value);
                foreach (var wb in batchesForProduct)
                {
                    if (remainingToPick <= 0) break;

                    int pickFromThis = Math.Min(wb.Quantity, remainingToPick);

                    pickedBatches.Add(new PickedBatch
                    {
                        BatchId = batch.Id,
                        WarehouseId = new WarehouseId(wb.WarehouseId),
                        PickedQuantity = pickFromThis
                    });

                    remainingToPick -= pickFromThis;
                }
            }

            return pickedBatches;
        }

        private class PickedBatch
        {
            public BatchId BatchId { get; set; }
            public WarehouseId WarehouseId { get; set; }
            public int PickedQuantity { get; set; }
        }
    }
}