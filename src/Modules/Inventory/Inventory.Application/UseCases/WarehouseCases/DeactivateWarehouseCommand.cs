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
/// Handles the command to deactivate a warehouse and returns the updated warehouse data.
/// </summary>
public class DeactivateWarehouseCommandHandler : IRequestHandler<DeactivateWarehouseCommandMediatR, WarehouseDto>
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly ITenantContext _tenantContext;

    public DeactivateWarehouseCommandHandler(IWarehouseRepository warehouseRepository, ITenantContext tenantContext)
    {
        _warehouseRepository = warehouseRepository;
        _tenantContext = tenantContext;
    }

    public async Task<WarehouseDto> Handle(DeactivateWarehouseCommandMediatR request, CancellationToken cancellationToken)
    {
        var warehouse = await _warehouseRepository.GetByIdAsync(WarehouseId.From(request.Contract.WarehouseId))
            ?? throw new KeyNotFoundException($"Warehouse '{request.Contract.WarehouseId}' was not found.");

        if (warehouse.BrandId != _tenantContext.BrandId)
            throw new KeyNotFoundException($"Warehouse '{request.Contract.WarehouseId}' was not found.");

        warehouse.Deactivate();
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