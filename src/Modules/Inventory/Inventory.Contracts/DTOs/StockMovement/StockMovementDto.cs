using System;

namespace Inventory.Contracts.DTOs.StockMovement
{
    public record StockMovementDto(
        Guid Id,
        Guid ProductId,
        Guid BatchId,
        Guid WarehouseId,
        Guid BrandId,
        int Quantity,
        int MovementType,
        int? ReferenceType,
        Guid? ReferenceId,
        DateTime CreatedAt,
        string? Reason = null
    );
}

