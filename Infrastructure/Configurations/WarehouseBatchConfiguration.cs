using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class WarehouseBatchConfiguration : IEntityTypeConfiguration<WarehouseBatch>
    {
        public void Configure(EntityTypeBuilder<WarehouseBatch> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_WarehouseBatches");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Quantity)
                .IsRequired();

            // Unique constraint on WarehouseId + BatchId
            builder.HasIndex(e => new { e.WarehouseId, e.BatchId }, "ix_warehouse_batches_warehouse_id_batch_id")
                .IsUnique();

            builder.HasOne(d => d.Batch).WithMany(p => p.WarehouseBatches)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_WarehouseBatches_Batches_BatchId");

            builder.HasOne(d => d.Warehouse).WithMany(p => p.WarehouseBatches)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_WarehouseBatches_Warehouses_WarehouseId");

            builder.HasOne(d => d.Brand).WithMany()
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
