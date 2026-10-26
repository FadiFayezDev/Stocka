using Inventory.Contracts.DTOs.Batch;

namespace Inventory.Contracts.UseCases.BatchCases;

/// <summary>
/// Full batch details preserving every quantity distinction (initial / unallocated / in warehouses).
/// </summary>
/// <param name="BatchId">The unique identifier of the batch to retrieve.</param>
public record RetrieveBatchQuery(Guid BatchId);