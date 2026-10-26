using Inventory.Contracts.DTOs.Warehouse;

namespace Inventory.Contracts.UseCases.WarehouseCases;

/// <summary>
/// Warehouses whose name contains the search value (existing name-matching convention).
/// </summary>
/// <param name="Name">The name search value; matched case-insensitively by containment.</param>
public record RetrieveWarehousesByNameQuery(string Name);