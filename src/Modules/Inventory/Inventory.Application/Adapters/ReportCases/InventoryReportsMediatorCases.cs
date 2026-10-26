using BuildingBlocks.Application.Common.Models;
using Inventory.Application.Bindings.ReportCases;
using Inventory.Contracts.DTOs.Batch;
using Inventory.Contracts.DTOs.Reports;
using Inventory.Contracts.DTOs.StockMovement;
using Inventory.Contracts.UseCases.ReportCases;
using MediatR;

namespace Inventory.Application.Adapters.ReportCases;

/// <summary>
/// Implements the IInventoryReportCases contract by dispatching each pure contract record
/// through the application's MediatR pipeline to the central report handler.
/// Registered in DI as the implementation of the module boundary contract.
/// </summary>
public class InventoryReportsMediatorCases : IInventoryReportCases
{
    private readonly IMediator _mediator;

    public InventoryReportsMediatorCases(IMediator mediator) => _mediator = mediator;

    public Task<IEnumerable<WarehouseStockSummaryDto>> GetWarehouseStockSummaryAsync(WarehouseStockSummaryQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new WarehouseStockSummaryQueryMediatR(query), cancellationToken);

    public Task<PaginatedResult<WarehouseStockSummaryDto>> GetWarehousesRankedByStockAsync(WarehousesRankedByStockQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new WarehousesRankedByStockQueryMediatR(query), cancellationToken);

    public Task<PaginatedResult<WarehouseStockSummaryDto>> GetWarehousesRankedByCostAsync(WarehousesRankedByCostQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new WarehousesRankedByCostQueryMediatR(query), cancellationToken);

    public Task<PaginatedResult<WarehouseInventoryItemDto>> GetWarehouseInventoryAsync(WarehouseInventoryQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new WarehouseInventoryQueryMediatR(query), cancellationToken);

    public Task<IEnumerable<WarehouseStockSummaryDto>> GetZeroStockWarehousesAsync(ZeroStockWarehousesQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new ZeroStockWarehousesQueryMediatR(query), cancellationToken);

    public Task<IEnumerable<WarehouseStockSummaryDto>> GetInactiveWarehousesWithStockAsync(InactiveWarehousesWithStockQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new InactiveWarehousesWithStockQueryMediatR(query), cancellationToken);

    public Task<PaginatedResult<ZeroStockProductDto>> GetZeroStockProductsAsync(ZeroStockProductsQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new ZeroStockProductsQueryMediatR(query), cancellationToken);

    public Task<IEnumerable<BatchDto>> GetBatchesWithZeroRemainingAsync(BatchesWithZeroRemainingQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new BatchesWithZeroRemainingQueryMediatR(query), cancellationToken);

    public Task<IEnumerable<MovementTypeTotalsDto>> GetMovementTypeTotalsAsync(MovementTypeTotalsQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new MovementTypeTotalsQueryMediatR(query), cancellationToken);

    public Task<InboundOutboundSummaryDto> GetInboundOutboundTotalsAsync(InboundOutboundTotalsQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new InboundOutboundTotalsQueryMediatR(query), cancellationToken);

    public Task<PaginatedResult<TransferHistoryDto>> GetTransferHistoryAsync(TransferHistoryQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new TransferHistoryQueryMediatR(query), cancellationToken);

    public Task<PaginatedResult<StockMovementDto>> GetAdjustmentHistoryAsync(AdjustmentHistoryQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new AdjustmentHistoryQueryMediatR(query), cancellationToken);

    public Task<PaginatedResult<HistoricalBalanceDto>> GetHistoricalBalanceAsync(HistoricalBalanceQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new HistoricalBalanceQueryMediatR(query), cancellationToken);

    public Task<InventorySummaryDto> GetInventorySummaryAsync(InventorySummaryQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new InventorySummaryQueryMediatR(query), cancellationToken);
}