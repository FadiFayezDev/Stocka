using BuildingBlocks.Application.Common.Models;
using Inventory.Application.Common.Interfaces;
using Inventory.Contracts.DTOs.Batch;
using Inventory.Contracts.DTOs.StockMovement;
using Inventory.Contracts.UseCases.BatchCases;
using MediatR;

namespace Inventory.Application.Bindings.BatchCases;

public record RegisterBatchCommandMediatR(RegisterBatchCommand Contract) : ICommand<BatchDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record RetrieveBatchQueryMediatR(RetrieveBatchQuery Contract) : IRequest<BatchDetailsDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record ListBatchesQueryMediatR(ListBatchesQuery Contract) : IRequest<PaginatedResult<BatchDto>>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record ListUnallocatedBatchesQueryMediatR(ListUnallocatedBatchesQuery Contract) : IRequest<PaginatedResult<BatchDto>>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record RetrieveBatchMovementHistoryQueryMediatR(RetrieveBatchMovementHistoryQuery Contract) : IRequest<IEnumerable<StockMovementDto>>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}