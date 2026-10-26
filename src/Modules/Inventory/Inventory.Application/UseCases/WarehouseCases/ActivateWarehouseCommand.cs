using BuildingBlocks.Application.Abstractions;
using Inventory.Application.Bindings.WarehouseCases;
using Inventory.Application.Repositories.Commands;
using Inventory.Contracts.DTOs.Warehouse;
using Inventory.Contracts.UseCases.WarehouseCases;
using Inventory.Primitives.Identifiers;
using MediatR;

namespace Inventory.Application.UseCases.WarehouseCases;

#region Handler
/// <summary>
/// Handles the activation of a warehouse and returns the updated warehouse data transfer object.
/// </summary>
public class ActivateWarehouseCommandHandler : IRequestHandler<ActivateWarehouseCommandMediatR, WarehouseDto>
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly ITenantContext _tenantContext;

    public ActivateWarehouseCommandHandler(IWarehouseRepository warehouseRepository, ITenantContext tenantContext)
    {
        _warehouseRepository = warehouseRepository;
        _tenantContext = tenantContext;
    }

    public async Task<WarehouseDto> Handle(ActivateWarehouseCommandMediatR request, CancellationToken cancellationToken)
    {
        var warehouse = await _warehouseRepository.GetByIdAsync(WarehouseId.From(request.Contract.WarehouseId))
            ?? throw new KeyNotFoundException($"Warehouse '{request.Contract.WarehouseId}' was not found.");

        if (warehouse.BrandId != _tenantContext.BrandId)
            throw new KeyNotFoundException($"Warehouse '{request.Contract.WarehouseId}' was not found.");

        warehouse.Activate();
        await _warehouseRepository.UpdateAsync(warehouse);

        return new WarehouseDto(
            warehouse.Id.Value,
            warehouse.BrandId.Value,
            warehouse.Name,
            warehouse.Location,
            warehouse.Description,
            warehouse.IsActive,
            (int)warehouse.Type);
    }
}
#endregion