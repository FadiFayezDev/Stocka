using Inventory.Contracts.DTOs.Warehouse;

namespace Inventory.Contracts.UseCases.WarehouseCases;

/// <summary>
/// Represents a partial metadata update of an existing warehouse: only provided (non-null)
/// fields change; historical data and stock are untouched.
/// </summary>
/// <param name="WarehouseId">The unique identifier of the warehouse to update.</param>
/// <param name="Name">New name; null keeps the current value.</param>
/// <param name="Location">New location; null keeps the current value.</param>
/// <param name="Description">New description; null keeps the current value.</param>
/// <param name="Type">New type; null keeps the current value.</param>
public record UpdateWarehouseCommand
{
    public Guid WarehouseId { get; init; }
    public string? Name { get; init; }
    public string? Location { get; init; }
    public string? Description { get; init; }
    public int? Type { get; init; }
}