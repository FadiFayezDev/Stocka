using Domain.Entities.Products;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class WarehouseBranchConfiguration : IEntityTypeConfiguration<WarehouseBranch>
    {
        public void Configure(EntityTypeBuilder<WarehouseBranch> builder)
        {
            builder.ToTable("warehouse_branch");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasConversion(
                    id => id.Value,
                    value => new WarehouseBranchId(value))
                .ValueGeneratedNever();
            builder.Property(x => x.BrandId)
                .IsRequired();
            builder.Property(x => x.BranchId)
                .IsRequired();
            builder.Property(x => x.WarehouseId)
                .IsRequired();

            builder.Property(x => x.BrandId).HasConversion(
                    id => id.Value,
                    value => new BrandId(value));
            builder.Property(x => x.BranchId).HasConversion(
                    id => id.Value,
                    value => new BranchId(value));
            builder.Property(x => x.WarehouseId).HasConversion(
                    id => id.Value,
                    value => new WarehouseId(value));

            builder.HasIndex(x => new { x.BranchId, x.WarehouseId })
                .IsUnique();

            builder
                .HasOne(wb => wb.Warehouse)
                .WithMany(w => w.WarehouseBranches)
                .HasForeignKey(wb => wb.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(wb => wb.Branch)
                .WithMany(b => b.WarehouseBranches)
                .HasForeignKey(wb => wb.BranchId)
                .OnDelete(DeleteBehavior.Cascade);

             builder
                .HasOne(wb => wb.Brand)
                .WithMany()
                .HasForeignKey(wb => wb.BrandId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
