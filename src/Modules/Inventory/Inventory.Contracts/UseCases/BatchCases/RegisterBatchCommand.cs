using Inventory.Contracts.DTOs.Batch;

namespace Inventory.Contracts.UseCases.BatchCases;

/// <summary>
/// Registers a batch received from a purchasing document.
/// This is a RECEIPT only â€” it does NOT place units into a warehouse; stock enters
/// warehouses via AllocateStockCommand (distribution). The two are deliberately distinct.
/// ProductId/PurchaseItemId are soft (Guid) references to Catalog/Purchasing modules.
/// </summary>
/// <param name="ProductId">Soft reference to the Catalog product the batch belongs to.</param>
/// <param name="PurchaseItemId">Soft reference to the Purchasing document line this receipt came from.</param>
/// <param name="InitialQuantity">Units received into the batch (becomes the initial RemainingQuantity).</param>
/// <param name="UnitCost">Acquisition cost per unit used by valuation reports.</param>
public record RegisterBatchCommand
{
    public Guid ProductId { get; init; }
    public Guid PurchaseItemId { get; init; }
    public int InitialQuantity { get; init; }
    public decimal UnitCost { get; init; }
}