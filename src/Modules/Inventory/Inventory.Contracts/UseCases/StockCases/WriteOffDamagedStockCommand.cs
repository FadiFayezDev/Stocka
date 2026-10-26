using Inventory.Contracts.DTOs.StockMovement;

namespace Inventory.Contracts.UseCases.StockCases;

/// <summary>
/// Damaged-stock write-off: removes stock from a warehouse as AdjustmentOut with a required
/// reason. History is preserved (no deletion); cost impact stays a pure query (see valuation
/// reports) â€” Inventory does NOT post financial journal entries.
/// </summary>
public record WriteOffDamagedStockCommand
{
    /// <summary>The batch containing the damaged units.</summary>
    public Guid BatchId { get; init; }

    /// <summary>The warehouse the damaged units are removed from.</summary>
    public Guid WarehouseId { get; init; }

    /// <summary>Number of damaged units to write off.</summary>
    public int Quantity { get; init; }

    /// <summary>Damage description â€” required for auditability.</summary>
    public string Reason { get; init; } = default!;

    /// <summary>Optional soft reference linking the write-off to an external document.</summary>
    public Guid? ReferenceId { get; init; }
}