using BuildingBlocks.Application.Common.Models;
using Inventory.Application.Bindings.StockCases;
using Inventory.Contracts.DTOs.Reports;
using Inventory.Contracts.DTOs.Stock;
using Inventory.Contracts.DTOs.StockMovement;
using Inventory.Contracts.UseCases.StockCases;
using MediatR;

namespace Inventory.Application.Adapters.StockCases;

/// <summary>
/// Implements the IStockCases contract by dispatching each pure contract record through the
/// application's MediatR pipeline (validation behavior, unit-of-work behavior, handler).
/// Registered in DI as the implementation of the module boundary contract.
/// </summary>
public class StockMediatorCases : IStockCases
{
    private readonly IMediator _mediator;

    public StockMediatorCases(IMediator mediator) => _mediator = mediator;

    public Task<AdjustmentResultDto> AdjustStockAsync(AdjustStockCommand command, CancellationToken cancellationToken = default)
        => _mediator.Send(new AdjustStockCommandMediatR(command), cancellationToken);

    public Task<AllocateStockResultDto> AllocateStockAsync(AllocateStockCommand command, CancellationToken cancellationToken = default)
        => _mediator.Send(new AllocateStockCommandMediatR(command), cancellationToken);

    public Task<TransferStockResultDto> TransferStockAsync(TransferStockCommand command, CancellationToken cancellationToken = default)
        => _mediator.Send(new TransferStockCommandMediatR(command), cancellationToken);

    public Task<StockMovementDto> WriteOffDamagedStockAsync(WriteOffDamagedStockCommand command, CancellationToken cancellationToken = default)
        => _mediator.Send(new WriteOffDamagedStockCommandMediatR(command), cancellationToken);

    public Task<StockMovementDto> ReverseStockMovementAsync(ReverseStockMovementCommand command, CancellationToken cancellationToken = default)
        => _mediator.Send(new ReverseStockMovementCommandMediatR(command), cancellationToken);

    public Task<StockMovementDto> RecordSaleOutAsync(RecordSaleOutCommand command, CancellationToken cancellationToken = default)
        => _mediator.Send(new RecordSaleOutCommandMediatR(command), cancellationToken);

    public Task<StockMovementDto> RecordCustomerReturnAsync(RecordCustomerReturnCommand command, CancellationToken cancellationToken = default)
        => _mediator.Send(new RecordCustomerReturnCommandMediatR(command), cancellationToken);

    public Task<PaginatedResult<StockMovementDto>> ListMovementsAsync(ListMovementsQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new ListMovementsQueryMediatR(query), cancellationToken);

    public Task<ProductStockDistributionDto> GetProductStockAsync(GetProductStockQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new GetProductStockQueryMediatR(query), cancellationToken);

    public Task<int> GetProductUnallocatedUnitsAsync(GetProductUnallocatedUnitsQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new GetProductUnallocatedUnitsQueryMediatR(query), cancellationToken);

    public Task<int> GetBatchQuantityInWarehouseAsync(GetBatchQuantityInWarehouseQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new GetBatchQuantityInWarehouseQueryMediatR(query), cancellationToken);

    public Task<int> GetWarehouseTotalUnitsAsync(GetWarehouseTotalUnitsQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new GetWarehouseTotalUnitsQueryMediatR(query), cancellationToken);
}