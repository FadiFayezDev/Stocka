using BuildingBlocks.Application.Common.Models;
using Inventory.Application.Common.Interfaces;
using Inventory.Contracts.DTOs.Warehouse;
using Inventory.Contracts.UseCases.WarehouseCases;
using MediatR;

namespace Inventory.Application.Bindings.WarehouseCases;

public record ActivateWarehouseCommandMediatR(ActivateWarehouseCommand Contract) : ICommand<WarehouseDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record DeactivateWarehouseCommandMediatR(DeactivateWarehouseCommand Contract) : ICommand<WarehouseDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record RegisterWarehouseCommandMediatR(RegisterWarehouseCommand Contract) : ICommand<WarehouseDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record UpdateWarehouseCommandMediatR(UpdateWarehouseCommand Contract) : ICommand<WarehouseDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record RetrieveWarehouseQueryMediatR(RetrieveWarehouseQuery Contract) : IRequest<WarehouseDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record RetrieveWarehousesByNameQueryMediatR(RetrieveWarehousesByNameQuery Contract) : IRequest<IEnumerable<WarehouseDto>>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record ListWarehousesQueryMediatR(ListWarehousesQuery Contract) : IRequest<PaginatedResult<WarehouseDto>>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}