using System;

namespace Inventory.Contracts.DTOs.Stock
{
    /// <summary>Result of placing batch units into a warehouse.</summary>
    public record AllocateStockResultDto(
        Guid BatchId,
        Guid WarehouseId,
        int QuantityInWarehouse,
        int BatchRemainingQuantity
    );

    /// <summary>Result of a warehouse-to-warehouse transfer (always one Business operation).</summary>
    public record TransferStockResultDto(
        Guid TransferReferenceId,
        Guid ProductId,
        Guid BatchId,
        Guid SourceWarehouseId,
        Guid DestinationWarehouseId,
        int QuantityInSource,
        int QuantityInDestination
    );

    /// <summary>Result of a physical-count adjustment.</summary>
    public record AdjustmentResultDto(
        Guid BatchId,
        Guid WarehouseId,
        int CountedQuantity,
        int PreviousQuantity,
        int QuantityChange,
        Guid? MovementId
    );
}

