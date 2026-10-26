using BuildingBlocks.Application.Common.Models;
using Inventory.Application.Bindings.ReportCases;
using Inventory.Application.Repositories.Queries;
using Inventory.Contracts.DTOs.Batch;
using Inventory.Contracts.DTOs.Reports;
using Inventory.Contracts.DTOs.StockMovement;
using Inventory.Contracts.UseCases.ReportCases;
using MediatR;

namespace Inventory.Application.UseCases.ReportCases;

/// <summary>
/// Central report read-model handler: each Handle maps exactly one report query onto its
/// dedicated repository method, always scoped to the current Brand, with paging bounds clamped
/// in one place (the Page/Size helpers).
/// </summary>
public class ReportQueryHandlers :
    IRequestHandler<WarehouseStockSummaryQueryMediatR, IEnumerable<WarehouseStockSummaryDto>>,
    IRequestHandler<WarehousesRankedByStockQueryMediatR, PaginatedResult<WarehouseStockSummaryDto>>,
    IRequestHandler<WarehousesRankedByCostQueryMediatR, PaginatedResult<WarehouseStockSummaryDto>>,
    IRequestHandler<WarehouseInventoryQueryMediatR, PaginatedResult<WarehouseInventoryItemDto>>,
    IRequestHandler<ZeroStockWarehousesQueryMediatR, IEnumerable<WarehouseStockSummaryDto>>,
    IRequestHandler<InactiveWarehousesWithStockQueryMediatR, IEnumerable<WarehouseStockSummaryDto>>,
    IRequestHandler<ZeroStockProductsQueryMediatR, PaginatedResult<ZeroStockProductDto>>,
    IRequestHandler<BatchesWithZeroRemainingQueryMediatR, IEnumerable<BatchDto>>,
    IRequestHandler<MovementTypeTotalsQueryMediatR, IEnumerable<MovementTypeTotalsDto>>,
    IRequestHandler<InboundOutboundTotalsQueryMediatR, InboundOutboundSummaryDto>,
    IRequestHandler<TransferHistoryQueryMediatR, PaginatedResult<TransferHistoryDto>>,
    IRequestHandler<AdjustmentHistoryQueryMediatR, PaginatedResult<StockMovementDto>>,
    IRequestHandler<HistoricalBalanceQueryMediatR, PaginatedResult<HistoricalBalanceDto>>,
    IRequestHandler<InventorySummaryQueryMediatR, InventorySummaryDto>
{
    private readonly IInventoryReportQueryRepository _reportRepository;
    private readonly BuildingBlocks.Application.Abstractions.ITenantContext _tenantContext;

    public ReportQueryHandlers(IInventoryReportQueryRepository reportRepository, BuildingBlocks.Application.Abstractions.ITenantContext tenantContext)
    {
        _reportRepository = reportRepository;
        _tenantContext = tenantContext;
    }

    private BuildingBlocks.Application.Primitives.Identifiers.BrandId BrandId => _tenantContext.BrandId;

    private static int Page(int page) => Math.Max(1, page);
    private static int Size(int pageSize) => Math.Clamp(pageSize, 1, 100);

    public async Task<IEnumerable<WarehouseStockSummaryDto>> Handle(WarehouseStockSummaryQueryMediatR request, CancellationToken ct)
        => await _reportRepository.GetWarehouseStockSummaryAsync(BrandId);

    public async Task<PaginatedResult<WarehouseStockSummaryDto>> Handle(WarehousesRankedByStockQueryMediatR request, CancellationToken ct)
        => await _reportRepository.GetWarehousesRankedByStockAsync(BrandId, request.Contract.Descending, Page(request.Contract.Page), Size(request.Contract.PageSize));

    public async Task<PaginatedResult<WarehouseStockSummaryDto>> Handle(WarehousesRankedByCostQueryMediatR request, CancellationToken ct)
        => await _reportRepository.GetWarehousesRankedByCostAsync(BrandId, request.Contract.Descending, Page(request.Contract.Page), Size(request.Contract.PageSize));

    public async Task<PaginatedResult<WarehouseInventoryItemDto>> Handle(WarehouseInventoryQueryMediatR request, CancellationToken ct)
        => await _reportRepository.GetWarehouseInventoryAsync(BrandId, request.Contract.WarehouseId, Page(request.Contract.Page), Size(request.Contract.PageSize));

    public async Task<IEnumerable<WarehouseStockSummaryDto>> Handle(ZeroStockWarehousesQueryMediatR request, CancellationToken ct)
        => await _reportRepository.GetZeroStockWarehousesAsync(BrandId);

    public async Task<IEnumerable<WarehouseStockSummaryDto>> Handle(InactiveWarehousesWithStockQueryMediatR request, CancellationToken ct)
        => await _reportRepository.GetInactiveWarehousesWithStockAsync(BrandId);

    public async Task<PaginatedResult<ZeroStockProductDto>> Handle(ZeroStockProductsQueryMediatR request, CancellationToken ct)
        => await _reportRepository.GetZeroStockProductsAsync(BrandId, Page(request.Contract.Page), Size(request.Contract.PageSize));

    public async Task<IEnumerable<BatchDto>> Handle(BatchesWithZeroRemainingQueryMediatR request, CancellationToken ct)
        => await _reportRepository.GetBatchesWithZeroRemainingAsync(BrandId);

    public async Task<IEnumerable<MovementTypeTotalsDto>> Handle(MovementTypeTotalsQueryMediatR request, CancellationToken ct)
        => await _reportRepository.GetMovementTotalsByTypeAsync(BrandId, request.Contract.DateFromUtc, request.Contract.DateToUtc);

    public async Task<InboundOutboundSummaryDto> Handle(InboundOutboundTotalsQueryMediatR request, CancellationToken ct)
    {
        if (request.Contract.DateFromUtc > request.Contract.DateToUtc)
            throw new ArgumentException("Date range is invalid: start must not be after end.", nameof(request));
        return await _reportRepository.GetInboundOutboundTotalsAsync(BrandId, request.Contract.DateFromUtc, request.Contract.DateToUtc);
    }

    public async Task<PaginatedResult<TransferHistoryDto>> Handle(TransferHistoryQueryMediatR request, CancellationToken ct)
        => await _reportRepository.GetTransferHistoryAsync(BrandId, Page(request.Contract.Page), Size(request.Contract.PageSize), request.Contract.DateFromUtc, request.Contract.DateToUtc);

    public async Task<PaginatedResult<StockMovementDto>> Handle(AdjustmentHistoryQueryMediatR request, CancellationToken ct)
        => await _reportRepository.GetAdjustmentHistoryAsync(BrandId, Page(request.Contract.Page), Size(request.Contract.PageSize), request.Contract.DateFromUtc, request.Contract.DateToUtc);

    public async Task<PaginatedResult<HistoricalBalanceDto>> Handle(HistoricalBalanceQueryMediatR request, CancellationToken ct)
        => await _reportRepository.GetHistoricalBalanceAsync(BrandId, request.Contract.AsOfUtc, Page(request.Contract.Page), Size(request.Contract.PageSize));

    public async Task<InventorySummaryDto> Handle(InventorySummaryQueryMediatR request, CancellationToken ct)
    {
        var brandId = BrandId;
        var (totalWarehouses, activeWarehouses, totalBatches, exhaustedBatches, unitsInWarehouses, unallocatedUnits, totalCost) =
            await allScalars();
        // Aggregate the scalars in one shot for a single round-trip experience per metric.
        return new InventorySummaryDto(brandId.Value, totalWarehouses, activeWarehouses, totalBatches, exhaustedBatches, unitsInWarehouses, unallocatedUnits, totalCost, DateTime.UtcNow);
    }

    private async Task<(int, int, int, int, int, int, decimal)> allScalars()
    {
        var totalWarehouses = await _reportRepository.GetWarehouseCountAsync(BrandId);
        var activeWarehouses = await _reportRepository.GetActiveWarehouseCountAsync(BrandId);
        var totalBatches = await _reportRepository.GetBatchCountAsync(BrandId);
        var exhaustedBatches = await _reportRepository.GetExhaustedBatchCountAsync(BrandId);
        var unitsInWarehouses = await _reportRepository.GetTotalUnitsInWarehousesAsync(BrandId);
        var unallocatedUnits = await _reportRepository.GetTotalUnallocatedUnitsAsync(BrandId);
        var totalCost = await _reportRepository.GetTotalInventoryCostAsync(BrandId);
        return (totalWarehouses, activeWarehouses, totalBatches, exhaustedBatches, unitsInWarehouses, unallocatedUnits, totalCost);
    }
}