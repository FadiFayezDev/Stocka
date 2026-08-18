using Domain.Bases;
using Domain.Entities.Core;
using Domain.Primitives;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Products
{
    public class WarehouseBranch : Entity<WarehouseBranchId>, IMultiTenantEntity
    {
        public BrandId BrandId { get; private set; }

        public BranchId BranchId { get; private set; }
        public Branch Branch { get; private set; } = null!;

        public WarehouseId WarehouseId { get; private set; }

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
