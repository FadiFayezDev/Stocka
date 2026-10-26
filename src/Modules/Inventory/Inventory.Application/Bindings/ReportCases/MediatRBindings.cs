using BuildingBlocks.Application.Common.Models;
using Inventory.Application.Common.Interfaces;
using Inventory.Contracts.DTOs.Batch;
using Inventory.Contracts.DTOs.Reports;
using Inventory.Contracts.DTOs.StockMovement;
using Inventory.Contracts.UseCases.ReportCases;
using MediatR;

namespace Inventory.Application.Bindings.ReportCases;

public record WarehouseStockSummaryQueryMediatR(WarehouseStockSummaryQuery Contract) : IRequest<IEnumerable<WarehouseStockSummaryDto>>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record WarehousesRankedByStockQueryMediatR(WarehousesRankedByStockQuery Contract) : IRequest<PaginatedResult<WarehouseStockSummaryDto>>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record WarehousesRankedByCostQueryMediatR(WarehousesRankedByCostQuery Contract) : IRequest<PaginatedResult<WarehouseStockSummaryDto>>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record WarehouseInventoryQueryMediatR(WarehouseInventoryQuery Contract) : IRequest<PaginatedResult<WarehouseInventoryItemDto>>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record ZeroStockWarehousesQueryMediatR(ZeroStockWarehousesQuery Contract) : IRequest<IEnumerable<WarehouseStockSummaryDto>>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record InactiveWarehousesWithStockQueryMediatR(InactiveWarehousesWithStockQuery Contract) : IRequest<IEnumerable<WarehouseStockSummaryDto>>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record ZeroStockProductsQueryMediatR(ZeroStockProductsQuery Contract) : IRequest<PaginatedResult<ZeroStockProductDto>>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record BatchesWithZeroRemainingQueryMediatR(BatchesWithZeroRemainingQuery Contract) : IRequest<IEnumerable<BatchDto>>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record MovementTypeTotalsQueryMediatR(MovementTypeTotalsQuery Contract) : IRequest<IEnumerable<MovementTypeTotalsDto>>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record InboundOutboundTotalsQueryMediatR(InboundOutboundTotalsQuery Contract) : IRequest<InboundOutboundSummaryDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record TransferHistoryQueryMediatR(TransferHistoryQuery Contract) : IRequest<PaginatedResult<TransferHistoryDto>>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record AdjustmentHistoryQueryMediatR(AdjustmentHistoryQuery Contract) : IRequest<PaginatedResult<StockMovementDto>>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record HistoricalBalanceQueryMediatR(HistoricalBalanceQuery Contract) : IRequest<PaginatedResult<HistoricalBalanceDto>>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record InventorySummaryQueryMediatR(InventorySummaryQuery Contract) : IRequest<InventorySummaryDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}