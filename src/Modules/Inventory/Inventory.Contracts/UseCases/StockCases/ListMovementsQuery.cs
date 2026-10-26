using BuildingBlocks.Application.Common.Models;
using Inventory.Contracts.DTOs.StockMovement;

namespace Inventory.Contracts.UseCases.StockCases;

/// <summary>
/// Paged movement history with optional filters (warehouse / batch / product / movement
/// types / date range). Aggregation and filtering happen in the database (Dapper).
/// </summary>
/// <remarks>
/// No dedicated validator: empty Guid filters and out-of-range paging are normalized in the
/// handler instead of failing the request.
/// </remarks>
public record ListMovementsQuery
{
    /// <summary>Optional warehouse filter (empty Guid is treated as no filter).</summary>
    public Guid? WarehouseId { get; init; }

    /// <summary>Optional batch filter (empty Guid is treated as no filter).</summary>
    public Guid? BatchId { get; init; }

    /// <summary>Optional product filter (empty Guid is treated as no filter).</summary>
    public Guid? ProductId { get; init; }

    /// <summary>Optional movement-type filter (int values of StockMovementType).</summary>
    public IReadOnlyList<int>? MovementTypes { get; init; }

    /// <summary>Inclusive lower bound of the movement date (UTC); null disables the bound.</summary>
    public DateTime? DateFromUtc { get; init; }

    /// <summary>Inclusive upper bound of the movement date (UTC); null disables the bound.</summary>
    public DateTime? DateToUtc { get; init; }

    /// <summary>Requested page number (clamped to â‰¥ 1).</summary>
    public int Page { get; init; } = 1;

    /// <summary>Page size (clamped to [1, 100]).</summary>
    public int PageSize { get; init; } = 20;
}