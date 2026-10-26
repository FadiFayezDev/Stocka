using Inventory.Contracts.DTOs.Warehouse;

namespace Inventory.Contracts.UseCases.WarehouseCases;

/// <summary>
/// Business rule (confirmed): deactivation is NON-DESTRUCTIVE and always permitted.
/// An inactive warehouse can still hold stock; historical movements are never deleted.
/// Inactive warehouses containing stock are surfaced by the control report.
/// </summary>
/// <param name="WarehouseId">The unique identifier of the warehouse to deactivate.</param>
public record DeactivateWarehouseCommand(Guid WarehouseId);