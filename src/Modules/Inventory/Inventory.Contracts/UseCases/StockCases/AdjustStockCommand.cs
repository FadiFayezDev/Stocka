using Inventory.Contracts.DTOs.Stock;

namespace Inventory.Contracts.UseCases.StockCases;

/// <summary>
/// Physical-count correction for one warehouse/batch placement. The movement direction is
/// derived from the count difference (AdjustmentIn for surplus, AdjustmentOut for shortage);
/// the recorded movement documents the delta against the counted quantity.
/// </summary>
public record AdjustStockCommand
{
    /// <summary>The batch being recounted.</summary>
    public Guid BatchId { get; init; }

    /// <summary>The warehouse placement being recounted.</summary>
    public Guid WarehouseId { get; init; }

    /// <summary>The quantity physically counted (may be equal to, above, or below the current quantity).</summary>
    public int CountedQuantity { get; init; }

    /// <summary>Physical-count reason â€” required for adjustments (auditability).</summary>
    public string Reason { get; init; } = default!;

    /// <summary>Optional soft reference linking the adjustment to an external document.</summary>
    public Guid? ReferenceId { get; init; }
}