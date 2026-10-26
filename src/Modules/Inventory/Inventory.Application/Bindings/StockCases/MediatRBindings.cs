using BuildingBlocks.Application.Common.Models;
using Inventory.Application.Common.Interfaces;
using Inventory.Contracts.DTOs.Reports;
using Inventory.Contracts.DTOs.Stock;
using Inventory.Contracts.DTOs.StockMovement;
using Inventory.Contracts.UseCases.StockCases;
using MediatR;

namespace Inventory.Application.Bindings.StockCases;

public record AdjustStockCommandMediatR(AdjustStockCommand Contract) : ICommand<AdjustmentResultDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record AllocateStockCommandMediatR(AllocateStockCommand Contract) : ICommand<AllocateStockResultDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record TransferStockCommandMediatR(TransferStockCommand Contract) : ICommand<TransferStockResultDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record WriteOffDamagedStockCommandMediatR(WriteOffDamagedStockCommand Contract) : ICommand<StockMovementDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record ReverseStockMovementCommandMediatR(ReverseStockMovementCommand Contract) : ICommand<StockMovementDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record RecordSaleOutCommandMediatR(RecordSaleOutCommand Contract) : ICommand<StockMovementDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record RecordCustomerReturnCommandMediatR(RecordCustomerReturnCommand Contract) : ICommand<StockMovementDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record ListMovementsQueryMediatR(ListMovementsQuery Contract) : IRequest<PaginatedResult<StockMovementDto>>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record GetProductStockQueryMediatR(GetProductStockQuery Contract) : IRequest<ProductStockDistributionDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record GetProductUnallocatedUnitsQueryMediatR(GetProductUnallocatedUnitsQuery Contract) : IRequest<int>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record GetBatchQuantityInWarehouseQueryMediatR(GetBatchQuantityInWarehouseQuery Contract) : IRequest<int>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record GetWarehouseTotalUnitsQueryMediatR(GetWarehouseTotalUnitsQuery Contract) : IRequest<int>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}