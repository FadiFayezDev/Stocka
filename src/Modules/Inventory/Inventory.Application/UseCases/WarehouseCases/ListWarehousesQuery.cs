using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Common.Models;
using Inventory.Application.Bindings.WarehouseCases;
using Inventory.Application.Repositories.Queries;
using Inventory.Contracts.DTOs.Warehouse;
using Inventory.Contracts.UseCases.WarehouseCases;
using MediatR;

namespace Inventory.Application.UseCases.WarehouseCases;

#region Handler
/// <summary>
/// Handles the paged warehouse listing: clamps the paging bounds and delegates to the query
/// repository for the current Brand, optionally filtered by activation state.
/// </summary>
public class ListWarehousesQueryHandler : IRequestHandler<ListWarehousesQueryMediatR, PaginatedResult<WarehouseDto>>
{
    private readonly IWarehouseQueryRepository _warehouseQueryRepository;
    private readonly ITenantContext _tenantContext;

    public ListWarehousesQueryHandler(IWarehouseQueryRepository warehouseQueryRepository, ITenantContext tenantContext)
    {
        _warehouseQueryRepository = warehouseQueryRepository;
        _tenantContext = tenantContext;
    }

    public async Task<PaginatedResult<WarehouseDto>> Handle(ListWarehousesQueryMediatR request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Contract.Page);
        var pageSize = Math.Clamp(request.Contract.PageSize, 1, 100);

        return request.Contract.IsActive.HasValue
            ? await _warehouseQueryRepository.GetPagedAsync(_tenantContext.BrandId, request.Contract.IsActive.Value, page, pageSize)
            : await _warehouseQueryRepository.GetPagedAsync(_tenantContext.BrandId, page, pageSize);
    }
}
#endregion