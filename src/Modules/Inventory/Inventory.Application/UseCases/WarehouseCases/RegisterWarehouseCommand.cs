using BuildingBlocks.Application.Abstractions;
using Inventory.Application.Bindings.WarehouseCases;
using Inventory.Application.Repositories.Commands;
using Inventory.Contracts.DTOs.Warehouse;
using Inventory.Contracts.UseCases.WarehouseCases;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using MediatR;

namespace Inventory.Application.UseCases.WarehouseCases;

#region Handler
/// <summary>
/// Creates the warehouse through the domain factory (which enforces name and type invariants)
/// and returns the created warehouse data transfer object.
/// </summary>
public class RegisterWarehouseCommandHandler : IRequestHandler<RegisterWarehouseCommandMediatR, WarehouseDto>
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly ITenantContext _tenantContext;

    public RegisterWarehouseCommandHandler(IWarehouseRepository warehouseRepository, ITenantContext tenantContext)
    {
        _warehouseRepository = warehouseRepository;
        _tenantContext = tenantContext;
    }

    public async Task<WarehouseDto> Handle(RegisterWarehouseCommandMediatR request, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.BrandId;

        var warehouse = Warehouse.Create(
            brandId,
            request.Contract.Name,
            (WarehouseType)request.Contract.Type,
            request.Contract.Location,
            request.Contract.Description);

        await _warehouseRepository.AddAsync(warehouse);

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