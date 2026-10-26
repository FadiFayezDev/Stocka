using BuildingBlocks.Application.Common.Models;
using Inventory.Application.Bindings.BatchCases;
using Inventory.Contracts.DTOs.Batch;
using Inventory.Contracts.DTOs.StockMovement;
using Inventory.Contracts.UseCases.BatchCases;
using MediatR;

namespace Inventory.Application.Adapters.BatchCases;

/// <summary>
/// Implements the IBatchCases contract by dispatching each pure contract record through the
/// application's MediatR pipeline (validation behavior, unit-of-work behavior, handler).
/// Registered in DI as the implementation of the module boundary contract.
/// </summary>
public class BatchMediatorCases : IBatchCases
{
    private readonly IMediator _mediator;

    public BatchMediatorCases(IMediator mediator) => _mediator = mediator;

    public Task<BatchDto> RegisterBatchAsync(RegisterBatchCommand command, CancellationToken cancellationToken = default)
        => _mediator.Send(new RegisterBatchCommandMediatR(command), cancellationToken);

    public Task<BatchDetailsDto> RetrieveBatchAsync(RetrieveBatchQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new RetrieveBatchQueryMediatR(query), cancellationToken);

    public Task<PaginatedResult<BatchDto>> ListBatchesAsync(ListBatchesQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new ListBatchesQueryMediatR(query), cancellationToken);

    public Task<PaginatedResult<BatchDto>> ListUnallocatedBatchesAsync(ListUnallocatedBatchesQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new ListUnallocatedBatchesQueryMediatR(query), cancellationToken);

    public Task<IEnumerable<StockMovementDto>> RetrieveBatchMovementHistoryAsync(RetrieveBatchMovementHistoryQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new RetrieveBatchMovementHistoryQueryMediatR(query), cancellationToken);
}