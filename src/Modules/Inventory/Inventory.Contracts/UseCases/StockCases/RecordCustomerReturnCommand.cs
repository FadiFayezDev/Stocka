using Inventory.Contracts.DTOs.StockMovement;

namespace Inventory.Contracts.UseCases.StockCases;

/// <summary>
/// Customer return into a warehouse (OrderReturn movement).
/// Approved rule: returned units go back to the SAME batch/warehouse they were sold from â€” they
/// re-enter CURRENT warehouse stock directly; the batch allocation bookkeeping
/// (RemainingQuantity) is NOT changed, because returns are not new allocatable receipts.
/// Classification of sellability is a future, per-return QA decision â€” none is invented here.
/// </summary>
public record RecordCustomerReturnCommand
{
    /// <summary>The batch/warehouse pair the units are returned into.</summary>
    public Guid BatchId { get; init; }

    /// <summary>The warehouse the units re-enter.</summary>
    public Guid WarehouseId { get; init; }

    /// <summary>Number of returned units.</summary>
    public int Quantity { get; init; }

    /// <summary>Soft reference to the original order (ReferenceType = Return when provided).</summary>
    public Guid? OrderReferenceId { get; init; }

    /// <summary>Optional audit note stored on the movement.</summary>
    public string? Reason { get; init; }
}