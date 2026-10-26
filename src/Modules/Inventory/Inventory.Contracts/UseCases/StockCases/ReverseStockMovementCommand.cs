using Inventory.Contracts.DTOs.StockMovement;

namespace Inventory.Contracts.UseCases.StockCases;

/// <summary>
/// Auditable correction of a previously recorded movement: an OPPOSITE-direction movement is
/// appended (never deleting or rewriting history), referencing the original movement
/// (ReferenceType = ReversalOfMovement). Transfer legs cannot be reversed in isolation â€”
/// the pair must be corrected with a counter-transfer.
/// Balancing effects: PurchaseIn reversal restores the batch's unallocated quantity;
/// other types only restore/withdraw current warehouse stock.
/// </summary>
public record ReverseStockMovementCommand
{
    /// <summary>The original movement being reversed.</summary>
    public Guid MovementId { get; init; }

    /// <summary>Required audit note explaining the correction.</summary>
    public string Reason { get; init; } = default!;
}