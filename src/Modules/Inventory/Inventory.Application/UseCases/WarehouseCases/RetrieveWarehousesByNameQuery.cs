using BuildingBlocks.Application.Abstractions;
using Inventory.Application.Bindings.WarehouseCases;
using Inventory.Application.Repositories.Queries;
using Inventory.Contracts.DTOs.Warehouse;
using Inventory.Contracts.UseCases.WarehouseCases;
using MediatR;

namespace Inventory.Application.UseCases.WarehouseCases;

#region Handler
/// <summary>
/// Handles the name search: returns the warehouses of the current Brand whose name contains the
/// trimmed search value.
/// </summary>
public class RetrieveWarehousesByNameQueryHandler : IRequestHandler<RetrieveWarehousesByNameQueryMediatR, IEnumerable<WarehouseDto>>
{
    private readonly IWarehouseQueryRepository _warehouseQueryRepository;
    private readonly ITenantContext _tenantContext;

    public RetrieveWarehousesByNameQueryHandler(IWarehouseQueryRepository warehouseQueryRepository, ITenantContext tenantContext)
    {
        _warehouseQueryRepository = warehouseQueryRepository;
        _tenantContext = tenantContext;
    }

    public async Task<IEnumerable<WarehouseDto>> Handle(RetrieveWarehousesByNameQueryMediatR request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Contract.Name))
            throw new ArgumentException("Warehouse name search value cannot be empty.", nameof(request.Contract.Name));

        return await _warehouseQueryRepository.GetByNameAsync(_tenantContext.BrandId, request.Contract.Name.Trim());
    }
}
#endregion