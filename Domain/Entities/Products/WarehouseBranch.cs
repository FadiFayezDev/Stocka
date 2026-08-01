using Domain.Bases;
using Domain.Entities.Core;
using Domain.Primitives;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Products
{
    public class WarehouseBranch : AggregateRoot<WarehouseBranchId>, IMultiTenantEntity
    {
        public BrandId BrandId { get; private set; }
        public virtual Brand Brand { get; private set; } = null!;

        public BranchId BranchId { get; private set; }
        public Branch Branch { get; private set; } = null!;

        public WarehouseId WarehouseId { get; private set; }
        public Warehouse Warehouse { get; private set; } = null!;

        private WarehouseBranch() { }

        [SetsRequiredMembers]
        public WarehouseBranch(BrandId brandId, BranchId branchId, WarehouseId warehouseId)
        {
            Id = WarehouseBranchId.New();
            BrandId = brandId;
            BranchId = branchId;
            WarehouseId = warehouseId;
        }

        Guid IMultiTenantEntity.BrandId
        {
            get => BrandId.Value;
            set => BrandId = new BrandId(value);
        }
    }
}
