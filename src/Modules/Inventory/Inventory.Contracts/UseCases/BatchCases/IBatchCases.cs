using BuildingBlocks.Application.Common.Models;
using Inventory.Contracts.DTOs.Batch;
using Inventory.Contracts.DTOs.StockMovement;

namespace Inventory.Contracts.UseCases.BatchCases;

/// <summary>
/// Batch use case contract: the pure module boundary for batch receipts and reads.
/// Implemented by the Inventory Application layer (which wires each call into its MediatR
/// pipeline — validation and unit-of-work included) and consumed via DI.
/// </summary>
public interface IBatchCases
{
    #region Commands
    /// <summary>Registers a batch received from a purchasing document (a receipt only — no placement).</summary>
    Task<BatchDto> RegisterBatchAsync(RegisterBatchCommand command, CancellationToken cancellationToken = default);
    #endregion

    #region Queries
    /// <summary>Retrieves full batch details preserving every quantity distinction.</summary>
    Task<BatchDetailsDto> RetrieveBatchAsync(RetrieveBatchQuery query, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a paged batch listing with optional product/purchase-item/warehouse filters.</summary>
    Task<PaginatedResult<BatchDto>> ListBatchesAsync(ListBatchesQuery query, CancellationToken cancellationToken = default);

    /// <summary>Retrieves batches whose units have not yet been allocated to any warehouse.</summary>
    Task<PaginatedResult<BatchDto>> ListUnallocatedBatchesAsync(ListUnallocatedBatchesQuery query, CancellationToken cancellationToken = default);

    /// <summary>Retrieves the complete chronological movement history of one batch (its audit trail).</summary>
    Task<IEnumerable<StockMovementDto>> RetrieveBatchMovementHistoryAsync(RetrieveBatchMovementHistoryQuery query, CancellationToken cancellationToken = default);
    #endregion
}