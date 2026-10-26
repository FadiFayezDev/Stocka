using BuildingBlocks.Application.Common.Models;
using Inventory.Application.Bindings.WarehouseCases;
using Inventory.Contracts.DTOs.Warehouse;
using Inventory.Contracts.UseCases.WarehouseCases;
using MediatR;

namespace Inventory.Application.Adapters.WarehouseCases;

/// <summary>
/// Implements the IWarehouseCases contract by dispatching each pure contract record through the
/// application's MediatR pipeline (validation behavior, unit-of-work behavior, handler).
/// Registered in DI as the implementation of the module boundary contract.
/// </summary>
public class WarehouseMediatorCases : IWarehouseCases
{
    private readonly IMediator _mediator;

    public WarehouseMediatorCases(IMediator mediator) => _mediator = mediator;

    public Task<WarehouseDto> ActivateWarehouseAsync(ActivateWarehouseCommand command, CancellationToken cancellationToken = default)
        => _mediator.Send(new ActivateWarehouseCommandMediatR(command), cancellationToken);

    public Task<WarehouseDto> DeactivateWarehouseAsync(DeactivateWarehouseCommand command, CancellationToken cancellationToken = default)
        => _mediator.Send(new DeactivateWarehouseCommandMediatR(command), cancellationToken);

    public Task<WarehouseDto> RegisterWarehouseAsync(RegisterWarehouseCommand command, CancellationToken cancellationToken = default)
        => _mediator.Send(new RegisterWarehouseCommandMediatR(command), cancellationToken);

    public Task<WarehouseDto> UpdateWarehouseAsync(UpdateWarehouseCommand command, CancellationToken cancellationToken = default)
        => _mediator.Send(new UpdateWarehouseCommandMediatR(command), cancellationToken);

    public Task<WarehouseDto> RetrieveWarehouseAsync(RetrieveWarehouseQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new RetrieveWarehouseQueryMediatR(query), cancellationToken);

    public Task<IEnumerable<WarehouseDto>> RetrieveWarehousesByNameAsync(RetrieveWarehousesByNameQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new RetrieveWarehousesByNameQueryMediatR(query), cancellationToken);

    public Task<PaginatedResult<WarehouseDto>> ListWarehousesAsync(ListWarehousesQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new ListWarehousesQueryMediatR(query), cancellationToken);
}