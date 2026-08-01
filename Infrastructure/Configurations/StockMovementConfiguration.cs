using Domain.Entities.Products;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
    {
        public void Configure(EntityTypeBuilder<StockMovement> builder)
        {
            builder.ToTable("stock_movements");

            builder.HasKey(e => e.Id).HasName("PK_StockMovements");

            builder.Property(e => e.Id).HasConversion(
                    id => id.Value,
                    value => new StockMovementId(value))
                .ValueGeneratedNever();
            builder.Property(e => e.ProductId).HasConversion(
                    id => id.Value,
                    value => new ProductId(value));
            builder.Property(e => e.BatchId).HasConversion(
                    id => id.Value,
                    value => new BatchId(value));
            builder.Property(e => e.WarehouseId).HasConversion(
                    id => id.Value,
                    value => new WarehouseId(value));
            builder.Property(e => e.BrandId).HasConversion(
                    id => id.Value,
                    value => new BrandId(value));
            builder.Property(e => e.Quantity)
                .IsRequired();
            builder.Property(e => e.MovementType)
                .HasConversion<string>()
                .HasMaxLength(50);
            builder.Property(e => e.ReferenceType)
                .HasMaxLength(50);
            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("NOW()");

            builder.HasOne(d => d.Batch).WithMany(p => p.StockMovements)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_StockMovements_Batches_BatchId");

            builder.HasOne(d => d.Product).WithMany(p => p.StockMovements)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_StockMovements_Products_ProductId");

            builder.HasOne(d => d.Warehouse).WithMany(p => p.StockMovements)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_StockMovements_Warehouses_WarehouseId");

            builder.HasOne(d => d.Brand).WithMany()
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
