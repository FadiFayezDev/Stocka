using BuildingBlocks.Application.Abstractions;
using Inventory.Application.Bindings.StockCases;
using Inventory.Application.Repositories.Commands;
using Inventory.Contracts.DTOs.Stock;
using Inventory.Contracts.UseCases.StockCases;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Inventory.Primitives.Identifiers;
using MediatR;

namespace Inventory.Application.UseCases.StockCases;

#region Handler
/// <summary>
/// Handles the physical-count correction: loads the batch with a Brand guard, computes the delta
/// between the counted quantity and the current placement quantity, and records a single
/// adjustment movement in the derived direction; recounting the same value is a verified no-op
/// that creates no movement.
/// </summary>
public class AdjustStockCommandHandler : IRequestHandler<AdjustStockCommandMediatR, AdjustmentResultDto>
{
    private readonly IBatchRepository _batchRepository;
    private readonly IWarehouseBatchRepository _warehouseBatchRepository;
    private readonly IStockMovementRepository _stockMovementRepository;
    private readonly ITenantContext _tenantContext;

    public AdjustStockCommandHandler(
        IBatchRepository batchRepository,
        IWarehouseBatchRepository warehouseBatchRepository,
        IStockMovementRepository stockMovementRepository,
        ITenantContext tenantContext)
    {
        _batchRepository = batchRepository;
        _warehouseBatchRepository = warehouseBatchRepository;
        _stockMovementRepository = stockMovementRepository;
        _tenantContext = tenantContext;
    }

    public async Task<AdjustmentResultDto> Handle(AdjustStockCommandMediatR request, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.BrandId;

        var batch = await _batchRepository.GetByIdAsync(BatchId.From(request.Contract.BatchId))
            ?? throw new KeyNotFoundException(StockOperationMessages.BatchNotFound);
        if (batch.BrandId != brandId) throw new KeyNotFoundException(StockOperationMessages.BatchNotFound);

        var warehouseBatch = await _warehouseBatchRepository.FindByWarehouseAndBatchAsync(
            WarehouseId.From(request.Contract.WarehouseId), batch.Id)
            ?? throw new InvalidOperationException(StockOperationMessages.BatchNotInWarehouse);

        var previousQuantity = warehouseBatch.Quantity;
        var delta = request.Contract.CountedQuantity - previousQuantity;

        // Idempotency: recounting the same value is a verified no-op — no duplicate movement is created.
        if (delta == 0)
            return new AdjustmentResultDto(batch.Id.Value, warehouseBatch.WarehouseId.Value, request.Contract.CountedQuantity, previousQuantity, 0, null);

        warehouseBatch.UpdateQuantity(request.Contract.CountedQuantity);

        StockMovement movement;
        if (delta > 0)
        {
            movement = new StockMovement(
                batch.ProductId, batch.Id, warehouseBatch.WarehouseId, brandId, delta,
                StockMovementType.AdjustmentIn, StockReferenceType.Adjustment, request.Contract.ReferenceId, request.Contract.Reason);
        }
        else
        {
            movement = new StockMovement(
                batch.ProductId, batch.Id, warehouseBatch.WarehouseId, brandId, -delta,
                StockMovementType.AdjustmentOut, StockReferenceType.Adjustment, request.Contract.ReferenceId, request.Contract.Reason);
        }

        await _warehouseBatchRepository.UpdateAsync(warehouseBatch);
        await _stockMovementRepository.AddAsync(movement);

        return new AdjustmentResultDto(
            batch.Id.Value,
            warehouseBatch.WarehouseId.Value,
            request.Contract.CountedQuantity,
            previousQuantity,
            delta,
            movement.Id.Value);
    }
}
#endregion