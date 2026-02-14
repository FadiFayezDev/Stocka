using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class WarehouseBatchConfiguration : IEntityTypeConfiguration<WarehouseBatch>
    {
        public void Configure(EntityTypeBuilder<WarehouseBatch> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_WarehouseBatches");

            builder.HasIndex(e => new { e.WarehouseId, e.BatchId }, "UQ_WarehouseBatches_WarehouseId_BatchId").IsUnique();

            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.HasOne(d => d.Batch).WithMany(p => p.WarehouseBatches)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarehouseBatches_Batches_BatchId");

            builder.HasOne(d => d.Warehouse).WithMany(p => p.WarehouseBatches)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarehouseBatches_Warehouses_WarehouseId");
        }
    }
}
