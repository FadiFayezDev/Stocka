using BuildingBlocks.Application.Abstractions;
using Inventory.Application.Bindings.StockCases;
using Inventory.Application.Repositories.Commands;
using Inventory.Contracts.DTOs.StockMovement;
using Inventory.Contracts.UseCases.StockCases;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Inventory.Primitives.Identifiers;
using MediatR;

namespace Inventory.Application.UseCases.StockCases;

#region Handler
/// <summary>
/// Handles the customer return: loads the batch with a Brand guard, adds the units back into the
/// same batch/warehouse placement, and records the OrderReturn movement with the Return
/// reference — allocation bookkeeping (RemainingQuantity) is deliberately left unchanged.
/// </summary>
public class RecordCustomerReturnCommandHandler : IRequestHandler<RecordCustomerReturnCommandMediatR, StockMovementDto>
{
    private readonly IBatchRepository _batchRepository;
    private readonly IWarehouseBatchRepository _warehouseBatchRepository;
    private readonly IStockMovementRepository _stockMovementRepository;
    private readonly ITenantContext _tenantContext;

    public RecordCustomerReturnCommandHandler(
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

    public async Task<StockMovementDto> Handle(RecordCustomerReturnCommandMediatR request, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.BrandId;

        var batch = await _batchRepository.GetByIdAsync(BatchId.From(request.Contract.BatchId))
            ?? throw new KeyNotFoundException(StockOperationMessages.BatchNotFound);
        if (batch.BrandId != brandId) throw new KeyNotFoundException(StockOperationMessages.BatchNotFound);

        var warehouseBatch = await _warehouseBatchRepository.FindByWarehouseAndBatchAsync(
            WarehouseId.From(request.Contract.WarehouseId), batch.Id)
            ?? throw new InvalidOperationException(StockOperationMessages.BatchNotInWarehouse);

        warehouseBatch.AddQuantity(request.Contract.Quantity);

        var movement = new StockMovement(
            batch.ProductId,
            batch.Id,
            warehouseBatch.WarehouseId,
            brandId,
            request.Contract.Quantity,
            StockMovementType.OrderReturn,
            StockReferenceType.Return,
            request.Contract.OrderReferenceId,
            request.Contract.Reason);

        await _warehouseBatchRepository.UpdateAsync(warehouseBatch);
        await _stockMovementRepository.AddAsync(movement);

        return RecordSaleOutCommandHandler.ToDto(movement);
    }
}
#endregion