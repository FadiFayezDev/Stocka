using Inventory.Contracts.DTOs.Warehouse;

namespace Inventory.Contracts.UseCases.WarehouseCases;

/// <summary>
/// Represents a command to activate a warehouse by its unique identifier.
/// </summary>
/// <param name="WarehouseId">The unique identifier of the warehouse to activate.</param>
public record ActivateWarehouseCommand(Guid WarehouseId);