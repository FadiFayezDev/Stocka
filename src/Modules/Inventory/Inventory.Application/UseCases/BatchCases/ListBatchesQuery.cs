using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Common.Models;
using Inventory.Application.Bindings.BatchCases;
using Inventory.Application.Repositories.Queries;
using Inventory.Contracts.DTOs.Batch;
using Inventory.Contracts.UseCases.BatchCases;
using Inventory.Primitives.Identifiers;
using MediatR;

namespace Inventory.Application.UseCases.BatchCases;

#region Handler
/// <summary>
/// Handles the batch listing: routes to the matching repository query â€” warehouse placement
/// (paged in memory), product filter, purchase-item filter, or the Brand-wide paged list.
/// </summary>
public class ListBatchesQueryHandler : IRequestHandler<ListBatchesQueryMediatR, PaginatedResult<BatchDto>>
{
    private readonly IBatchQueryRepository _batchQueryRepository;
    private readonly ITenantContext _tenantContext;

    public ListBatchesQueryHandler(IBatchQueryRepository batchQueryRepository, ITenantContext tenantContext)
    {
        _batchQueryRepository = batchQueryRepository;
        _tenantContext = tenantContext;
    }

    public async Task<PaginatedResult<BatchDto>> Handle(ListBatchesQueryMediatR request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Contract.Page);
        var pageSize = Math.Clamp(request.Contract.PageSize, 1, 100);
        var brandId = _tenantContext.BrandId;

        if (request.Contract.WarehouseId.HasValue && request.Contract.WarehouseId.Value != Guid.Empty)
        {
            var warehouseBatches = await _batchQueryRepository.GetByWarehouseAsync(brandId, WarehouseId.From(request.Contract.WarehouseId.Value));
            return warehouseBatches.ToPaginated(page, pageSize);
        }

        if (request.Contract.ProductId.HasValue && request.Contract.ProductId.Value != Guid.Empty)
            return await _batchQueryRepository.GetByProductAsync(brandId, request.Contract.ProductId.Value, page, pageSize);

        if (request.Contract.PurchaseItemId.HasValue && request.Contract.PurchaseItemId.Value != Guid.Empty)
            return await _batchQueryRepository.GetByPurchaseItemAsync(brandId, request.Contract.PurchaseItemId.Value, page, pageSize);

        return await _batchQueryRepository.GetAllPagedAsync(brandId, page, pageSize);
    }
}
#endregion