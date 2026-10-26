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
/// Handles the allocation: validates the batch's unallocated quantity and the warehouse's Brand/activity,
/// creates or increases the WarehouseBatch placement, deducts the batch's RemainingQuantity, and
/// records the PurchaseIn movement — all persisted by one SaveChanges via UnitOfWorkBehavior.
/// </summary>
public class AllocateStockCommandHandler : IRequestHandler<AllocateStockCommandMediatR, AllocateStockResultDto>
{
    private readonly IBatchRepository _batchRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IWarehouseBatchRepository _warehouseBatchRepository;
    private readonly IStockMovementRepository _stockMovementRepository;
    private readonly ITenantContext _tenantContext;

    public AllocateStockCommandHandler(
        IBatchRepository batchRepository,
        IWarehouseRepository warehouseRepository,
        IWarehouseBatchRepository warehouseBatchRepository,
        IStockMovementRepository stockMovementRepository,
        ITenantContext tenantContext)
    {
        _batchRepository = batchRepository;
        _warehouseRepository = warehouseRepository;
        _warehouseBatchRepository = warehouseBatchRepository;
        _stockMovementRepository = stockMovementRepository;
        _tenantContext = tenantContext;
    }

    public async Task<AllocateStockResultDto> Handle(AllocateStockCommandMediatR request, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.BrandId;

        var batch = await _batchRepository.GetByIdAsync(BatchId.From(request.Contract.BatchId))
            ?? throw new KeyNotFoundException(StockOperationMessages.BatchNotFound);
        if (batch.BrandId != brandId) throw new KeyNotFoundException(StockOperationMessages.BatchNotFound);

        if (request.Contract.Quantity > batch.RemainingQuantity)
            throw new InvalidOperationException(
                $"Cannot allocate {request.Contract.Quantity} units: batch has only {batch.RemainingQuantity} unallocated units left.");

        var warehouse = await _warehouseRepository.GetByIdAsync(WarehouseId.From(request.Contract.WarehouseId))
            ?? throw new KeyNotFoundException(StockOperationMessages.WarehouseNotFound);
        if (warehouse.BrandId != brandId) throw new KeyNotFoundException(StockOperationMessages.WarehouseNotFound);
        if (!warehouse.IsActive) throw new InvalidOperationException(StockOperationMessages.WarehouseInactive);

        var existingWarehouseBatch = await _warehouseBatchRepository.FindByWarehouseAndBatchAsync(warehouse.Id, batch.Id);
        WarehouseBatch warehouseBatch;
        if (existingWarehouseBatch == null)
        {
            warehouseBatch = new WarehouseBatch(warehouse.Id, batch.Id, brandId, request.Contract.Quantity);
            await _warehouseBatchRepository.AddAsync(warehouseBatch);
        }
        else
        {
            warehouseBatch = existingWarehouseBatch;
            warehouseBatch.AddQuantity(request.Contract.Quantity);
            await _warehouseBatchRepository.UpdateAsync(warehouseBatch);
        }

        batch.DeductQuantity(request.Contract.Quantity);

        await _batchRepository.UpdateAsync(batch);
        await _stockMovementRepository.AddAsync(new StockMovement(
            batch.ProductId,
            batch.Id,
            warehouse.Id,
            brandId,
            request.Contract.Quantity,
            StockMovementType.PurchaseIn,
            StockReferenceType.Purchase,
            batch.PurchaseItemId));

        return new AllocateStockResultDto(
            batch.Id.Value,
            warehouse.Id.Value,
            warehouseBatch.Quantity,
            batch.RemainingQuantity);
    }
}
#endregion
