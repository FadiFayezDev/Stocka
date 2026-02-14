using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ValueObjects
{
    public class WarehouseBatchKey
    {
        public Guid WarehouseId { get; set; }
        public Guid BatchId { get; set; }

        public WarehouseBatchKey(Guid warehouseId, Guid batchId) 
        {
            WarehouseId = warehouseId;
            BatchId = batchId;
        }

        public override bool Equals(object? obj)
        {
            if (obj is WarehouseBatchKey other)
            {
                return WarehouseId == other.WarehouseId && BatchId == other.BatchId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(WarehouseId, BatchId);
        }
    }
}