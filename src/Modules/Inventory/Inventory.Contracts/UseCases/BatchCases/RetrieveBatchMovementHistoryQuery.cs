using Inventory.Contracts.DTOs.StockMovement;

namespace Inventory.Contracts.UseCases.BatchCases;

/// <summary>
/// Complete (unbounded but chronological) movement history of one batch â€” its audit trail.
/// </summary>
/// <param name="BatchId">The unique identifier of the batch whose history is retrieved.</param>
public record RetrieveBatchMovementHistoryQuery(Guid BatchId);