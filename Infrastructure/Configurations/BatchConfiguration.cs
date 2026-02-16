using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class BatchConfiguration : IEntityTypeConfiguration<Batch>
    {
        public void Configure(EntityTypeBuilder<Batch> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Batches");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.UnitCost)
                .HasColumnType("decimal(18, 2)");
            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("NOW()");  // ? ????? ?? GETUTCDATE() ??? NOW()

            builder.HasOne(d => d.Product).WithMany(p => p.Batches)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Batches_Products_ProductId");

            builder.HasOne(d => d.PurchaseItem).WithMany(p => p.Batches)
                .HasForeignKey(d => d.PurchaseItemId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Batches_PurchaseItems_PurchaseItemId");

            builder.HasOne(d => d.Brand).WithMany()
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure OrderItems collection
            builder.HasMany(d => d.OrderItems)
                .WithOne(d => d.Batch)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure StockMovements collection
            builder.HasMany(d => d.StockMovements)
                .WithOne(d => d.Batch)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure WarehouseBatches collection
            builder.HasMany(d => d.WarehouseBatches)
                .WithOne(d => d.Batch)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}