using BuildingBlocks.Application.Common.Models;
using Inventory.Contracts.DTOs.Batch;

namespace Inventory.Contracts.UseCases.BatchCases;

/// <summary>
/// Paged batch listing with optional filters: product, purchase item, or a warehouse
/// the batch currently holds stock in. Exhausted/unallocated listings use dedicated queries.
/// </summary>
/// <remarks>
/// No dedicated validator: empty Guid filters are ignored and paging bounds are clamped
/// in the handler instead of failing the request.
/// </remarks>
public record ListBatchesQuery
{
    /// <summary>Optional product filter (empty Guid is treated as no filter).</summary>
    public Guid? ProductId { get; init; }

    /// <summary>Optional purchase-item filter (empty Guid is treated as no filter).</summary>
    public Guid? PurchaseItemId { get; init; }

    /// <summary>Optional warehouse filter; provided, it precedes the other filters (empty Guid = no filter).</summary>
    public Guid? WarehouseId { get; init; }

    /// <summary>Requested page number (clamped to â‰¥ 1).</summary>
    public int Page { get; init; } = 1;

    /// <summary>Page size (clamped to [1, 100]).</summary>
    public int PageSize { get; init; } = 20;
}