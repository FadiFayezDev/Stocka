using BuildingBlocks.Application.Abstractions;
using Inventory.Application.Bindings.BatchCases;
using Inventory.Application.Repositories.Commands;
using Inventory.Contracts.DTOs.Batch;
using Inventory.Contracts.UseCases.BatchCases;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.UseCases.BatchCases;

#region Handler
/// <summary>
/// Handles the registration: creates the batch entity through its constructor (which stamps the
/// current Brand) and returns the batch data transfer object. No stock placement happens here.
/// </summary>
public class RegisterBatchCommandHandler : IRequestHandler<RegisterBatchCommandMediatR, BatchDto>
{
    private readonly IBatchRepository _batchRepository;
    private readonly ITenantContext _tenantContext;

    public RegisterBatchCommandHandler(IBatchRepository batchRepository, ITenantContext tenantContext)
    {
        _batchRepository = batchRepository;
        _tenantContext = tenantContext;
    }

    public async Task<BatchDto> Handle(RegisterBatchCommandMediatR request, CancellationToken cancellationToken)
    {
        var batch = new Batch(
            request.Contract.PurchaseItemId,
            request.Contract.ProductId,
            _tenantContext.BrandId,
            request.Contract.InitialQuantity,
            request.Contract.UnitCost);

        await _batchRepository.AddAsync(batch);

        return new BatchDto(
            batch.Id.Value,
            batch.ProductId,
            batch.PurchaseItemId,
            batch.BrandId.Value,
            batch.InitialQuantity,
            batch.RemainingQuantity,
            batch.UnitCost,
            batch.CreatedAt);
    }
}
#endregion