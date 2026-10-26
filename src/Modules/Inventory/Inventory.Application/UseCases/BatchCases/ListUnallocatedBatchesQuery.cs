using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Common.Models;
using Inventory.Application.Bindings.BatchCases;
using Inventory.Application.Repositories.Queries;
using Inventory.Contracts.DTOs.Batch;
using Inventory.Contracts.UseCases.BatchCases;
using MediatR;

namespace Inventory.Application.UseCases.BatchCases;

#region Handler
/// <summary>
/// Handles the unallocated batch listing: fetches all unallocated batches of the current Brand
/// and pages them in memory.
/// </summary>
public class ListUnallocatedBatchesQueryHandler : IRequestHandler<ListUnallocatedBatchesQueryMediatR, PaginatedResult<BatchDto>>
{
    private readonly IBatchQueryRepository _batchQueryRepository;
    private readonly ITenantContext _tenantContext;

    public ListUnallocatedBatchesQueryHandler(IBatchQueryRepository batchQueryRepository, ITenantContext tenantContext)
    {
        _batchQueryRepository = batchQueryRepository;
        _tenantContext = tenantContext;
    }

    public async Task<PaginatedResult<BatchDto>> Handle(ListUnallocatedBatchesQueryMediatR request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Contract.Page);
        var pageSize = Math.Clamp(request.Contract.PageSize, 1, 100);
        var batches = await _batchQueryRepository.GetUnallocatedAsync(_tenantContext.BrandId);
        return batches.ToPaginated(page, pageSize);
    }
}
#endregion