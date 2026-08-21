using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.QueryRepositories;
using Domain.Entities.Products;
using Domain.Enums;
using Domain.Primitives;
using Domain.Repositories.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.WarehouseCases
{
    public class TransferStockCommand : IRequest<TransferStockResultDto>
    {
        public Guid ProductId { get; set; }
        public Guid FromWarehouseId { get; set; }
        public Guid ToWarehouseId { get; set; }
        public int Quantity { get; set; }
        public string? Notes { get; set; }
    }

    public class TransferStockResultDto
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public int MovedQuantity { get; set; }
        public Guid FromWarehouseId { get; set; }
        public Guid ToWarehouseId { get; set; }
    }

    public class TransferStockCommandHandler : IRequestHandler<TransferStockCommand, TransferStockResultDto>
    {
        private readonly ICurrentUserContext _currentUser;
        private readonly IWarehouseCommandRepository _warehouseCommand;
        private readonly IBatchCommandRepository _batchRepo;
        private readonly IStockMovementCommandRepository _stockMovementRepo;
        private readonly IProductQueryRepository _productQuery;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TransferStockCommandHandler> _logger;

        public TransferStockCommandHandler(
            ICurrentUserContext currentUser,
            IWarehouseCommandRepository warehouseCommand,
            IBatchCommandRepository batchRepo,
            IStockMovementCommandRepository stockMovementRepo,
            IProductQueryRepository productQuery,
            IUnitOfWork unitOfWork,
            ILogger<TransferStockCommandHandler> logger)
        {
            _currentUser = currentUser;
            _warehouseCommand = warehouseCommand;
            _batchRepo = batchRepo;
            _stockMovementRepo = stockMovementRepo;
            _productQuery = productQuery;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<TransferStockResultDto> Handle(TransferStockCommand request, CancellationToken cancellationToken)
        {
            if (request.Quantity <= 0)
                throw new BadRequestException("كمية التحويل يجب أن تكون أكبر من صفر.");

            if (request.FromWarehouseId == request.ToWarehouseId)
                throw new BadRequestException("لا يمكن التحويل من المخزن إلى نفسه.");

            var brandId = _currentUser.ActiveBrandId;

            var fromWarehouse = await _warehouseCommand.GetByIdAsync(request.FromWarehouseId);
            var toWarehouse = await _warehouseCommand.GetByIdAsync(request.ToWarehouseId);

            if (fromWarehouse == null || toWarehouse == null ||
                fromWarehouse.BrandId.Value != brandId || toWarehouse.BrandId.Value != brandId)
                throw new NotFoundException("أحد المخازن غير موجود في نطاق العلامة.");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var batches = (await _batchRepo.GetAvailableBatchesForProductAsync(request.ProductId, brandId))
                    .Where(b => b.RemainingQuantity > 0)
                    .OrderBy(b => b.CreatedAt)
                    .ToList();

                int remainingToMove = request.Quantity;
                var picked = new List<(BatchId BatchId, int Qty)>();

                foreach (var batch in batches)
                {
                    if (remainingToMove <= 0) break;

                    var wb = fromWarehouse.WarehouseBatches
                        .FirstOrDefault(x => x.BatchId == batch.Id && x.Quantity > 0);

                    if (wb == null) continue;

                    int move = Math.Min(wb.Quantity, remainingToMove);

                    fromWarehouse.UpdateBatchQuantity(wb.Id, wb.Quantity - move);

                    var existingTarget = toWarehouse.WarehouseBatches
                        .FirstOrDefault(x => x.BatchId == batch.Id);
                    if (existingTarget != null)
                        existingTarget.AddQuantity(move);
                    else
                        toWarehouse.AddBatch(batch.Id, new BrandId(brandId), move);

                    await _stockMovementRepo.CreateAsync(new StockMovement(
                        new ProductId(request.ProductId),
                        batch.Id,
                        fromWarehouse.Id,
                        new BrandId(brandId),
                        move,
                        StockMovementType.TransferOut,
                        StockReferenceType.Transfer,
                        toWarehouse.Id.Value));

                    await _stockMovementRepo.CreateAsync(new StockMovement(
                        new ProductId(request.ProductId),
                        batch.Id,
                        toWarehouse.Id,
                        new BrandId(brandId),
                        move,
                        StockMovementType.TransferIn,
                        StockReferenceType.Transfer,
                        fromWarehouse.Id.Value));

                    picked.Add((batch.Id, move));
                    remainingToMove -= move;
                }

                if (picked.Sum(p => p.Qty) < request.Quantity)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new BadRequestException("الكمية المطلوبة غير متوفرة في مخزن التحويل. الكمية المتاحة: "
                        + picked.Sum(p => p.Qty) + ".");
                }

                await _warehouseCommand.UpdateAsync(fromWarehouse);
                await _warehouseCommand.UpdateAsync(toWarehouse);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync();

                var product = await _productQuery.GetByIdAsync(request.ProductId);

                _logger.LogInformation("Stock transferred from {From} to {To}: {Qty} of product {Product}",
                    request.FromWarehouseId, request.ToWarehouseId, request.Quantity, request.ProductId);

                return new TransferStockResultDto
                {
                    ProductId = request.ProductId,
                    ProductName = product?.Name,
                    MovedQuantity = request.Quantity,
                    FromWarehouseId = request.FromWarehouseId,
                    ToWarehouseId = request.ToWarehouseId
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Stock transfer failed");
                throw;
            }
        }
    }
}