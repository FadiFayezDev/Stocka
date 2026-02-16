using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
    {
        public void Configure(EntityTypeBuilder<StockMovement> builder)
        {
            builder.ToTable("StockMovements");

            builder.HasKey(e => e.Id).HasName("PK_StockMovements");

            builder.Property(e => e.Id).ValueGeneratedNever();
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
