using BuildingBlocks.Application.Common.Models;
using Inventory.Contracts.DTOs.Batch;

namespace Inventory.Contracts.UseCases.BatchCases;

/// <summary>
/// Batches whose units have NOT yet been allocated to any warehouse (RemainingQuantity &gt; 0).
/// </summary>
/// <param name="Page">Requested page number (clamped to â‰¥ 1).</param>
/// <param name="PageSize">Page size (clamped to [1, 100]).</param>
public record ListUnallocatedBatchesQuery(int Page = 1, int PageSize = 20);