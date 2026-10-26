using BuildingBlocks.Application.Abstractions;
using Inventory.Application.Bindings.StockCases;
using Inventory.Application.Repositories.Queries;
using Inventory.Contracts.DTOs.Reports;
using Inventory.Contracts.UseCases.StockCases;
using Inventory.Primitives.Identifiers;
using MediatR;

namespace Inventory.Application.UseCases.StockCases;

#region GetProductStockQuery
/// <summary>
/// Handles the product stock distribution: aggregates the brand-wide distribution (optionally
/// filtered down to one warehouse) with the total counted units.
/// </summary>
public class GetProductStockQueryHandler : IRequestHandler<GetProductStockQueryMediatR, ProductStockDistributionDto>
{
    private readonly IInventoryReportQueryRepository _reportRepository;
    private readonly ITenantContext _tenantContext;

    public GetProductStockQueryHandler(IInventoryReportQueryRepository reportRepository, ITenantContext tenantContext)
    {
        _reportRepository = reportRepository;
        _tenantContext = tenantContext;
    }

    public async Task<ProductStockDistributionDto> Handle(GetProductStockQueryMediatR request, CancellationToken cancellationToken)
    {
        if (request.Contract.ProductId == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty.", nameof(request.Contract.ProductId));

        var brandId = _tenantContext.BrandId;

        var rows = await _reportRepository.GetProductStockDistributionAsync(brandId, request.Contract.ProductId);
        if (request.Contract.WarehouseId is { } wid && wid != Guid.Empty)
            rows = rows.Where(r => r.WarehouseId == wid).ToList();

        var total = await _reportRepository.GetProductTotalUnitsAsync(brandId, request.Contract.ProductId);

        return new ProductStockDistributionDto(request.Contract.ProductId, total, rows.ToList());
    }
}
#endregion

#region GetProductUnallocatedUnitsQuery
/// <summary>Handles the unallocated-units count for one product across all its batches.</summary>
public class GetProductUnallocatedUnitsQueryHandler : IRequestHandler<GetProductUnallocatedUnitsQueryMediatR, int>
{
    private readonly IInventoryReportQueryRepository _reportRepository;
    private readonly ITenantContext _tenantContext;

    public GetProductUnallocatedUnitsQueryHandler(IInventoryReportQueryRepository reportRepository, ITenantContext tenantContext)
    {
        _reportRepository = reportRepository;
        _tenantContext = tenantContext;
    }

    public async Task<int> Handle(GetProductUnallocatedUnitsQueryMediatR request, CancellationToken cancellationToken)
    {
        if (request.Contract.ProductId == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty.", nameof(request.Contract.ProductId));

        return await _reportRepository.GetProductUnallocatedUnitsAsync(_tenantContext.BrandId, request.Contract.ProductId);
    }
}
#endregion

#region GetBatchQuantityInWarehouseQuery
/// <summary>Handles the current quantity held by one batch in one warehouse.</summary>
public class GetBatchQuantityInWarehouseQueryHandler : IRequestHandler<GetBatchQuantityInWarehouseQueryMediatR, int>
{
    private readonly IWarehouseBatchQueryRepository _warehouseBatchQueryRepository;
    private readonly ITenantContext _tenantContext;

    public GetBatchQuantityInWarehouseQueryHandler(IWarehouseBatchQueryRepository warehouseBatchQueryRepository, ITenantContext tenantContext)
    {
        _warehouseBatchQueryRepository = warehouseBatchQueryRepository;
        _tenantContext = tenantContext;
    }

    public async Task<int> Handle(GetBatchQuantityInWarehouseQueryMediatR request, CancellationToken cancellationToken)
    {
        return await _warehouseBatchQueryRepository.GetQuantityInWarehouseAsync(
            _tenantContext.BrandId,
            WarehouseId.From(request.Contract.WarehouseId),
            BatchId.From(request.Contract.BatchId));
    }
}
#endregion

#region GetWarehouseTotalUnitsQuery
/// <summary>Handles the total current units held in one warehouse.</summary>
public class GetWarehouseTotalUnitsQueryHandler : IRequestHandler<GetWarehouseTotalUnitsQueryMediatR, int>
{
    private readonly IWarehouseBatchQueryRepository _warehouseBatchQueryRepository;
    private readonly ITenantContext _tenantContext;

    public GetWarehouseTotalUnitsQueryHandler(IWarehouseBatchQueryRepository warehouseBatchQueryRepository, ITenantContext tenantContext)
    {
        _warehouseBatchQueryRepository = warehouseBatchQueryRepository;
        _tenantContext = tenantContext;
    }

    public async Task<int> Handle(GetWarehouseTotalUnitsQueryMediatR request, CancellationToken cancellationToken)
    {
        if (request.Contract.WarehouseId == Guid.Empty)
            throw new ArgumentException("Warehouse ID cannot be empty.", nameof(request.Contract.WarehouseId));

        var placements = await _warehouseBatchQueryRepository.GetAllAsync(_tenantContext.BrandId, WarehouseId.From(request.Contract.WarehouseId));
        return placements.Sum(p => p.Quantity);
    }
}
#endregion