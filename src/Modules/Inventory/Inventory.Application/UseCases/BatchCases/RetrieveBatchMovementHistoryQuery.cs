using BuildingBlocks.Application.Abstractions;
using Inventory.Application.Bindings.BatchCases;
using Inventory.Application.Repositories.Queries;
using Inventory.Contracts.DTOs.StockMovement;
using Inventory.Contracts.UseCases.BatchCases;
using Inventory.Primitives.Identifiers;
using MediatR;

namespace Inventory.Application.UseCases.BatchCases;

#region Handler
/// <summary>
/// Handles the batch history retrieval: returns the batch's full chronological movement history
/// scoped to the current Brand (cross-Brand IDs are indistinguishable from not-found).
/// </summary>
public class RetrieveBatchMovementHistoryQueryHandler : IRequestHandler<RetrieveBatchMovementHistoryQueryMediatR, IEnumerable<StockMovementDto>>
{
    private readonly IStockMovementQueryRepository _stockMovementQueryRepository;
    private readonly ITenantContext _tenantContext;

    public RetrieveBatchMovementHistoryQueryHandler(IStockMovementQueryRepository stockMovementQueryRepository, ITenantContext tenantContext)
    {
        _stockMovementQueryRepository = stockMovementQueryRepository;
        _tenantContext = tenantContext;
    }

    public async Task<IEnumerable<StockMovementDto>> Handle(RetrieveBatchMovementHistoryQueryMediatR request, CancellationToken cancellationToken)
    {
        if (request.Contract.BatchId == Guid.Empty)
            throw new ArgumentException("Batch ID cannot be empty.", nameof(request.Contract.BatchId));

        // Existence check scoped to the current Brand (cross-Brand => not found).
        return await _stockMovementQueryRepository.GetByBatchAsync(_tenantContext.BrandId, BatchId.From(request.Contract.BatchId));
    }
}
#endregion