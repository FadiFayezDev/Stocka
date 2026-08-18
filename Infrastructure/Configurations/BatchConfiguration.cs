using Domain.Entities.Core;
using Domain.Entities.Products;
using Domain.Entities.Purchasing;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class BatchConfiguration : IEntityTypeConfiguration<Batch>
    {
        public void Configure(EntityTypeBuilder<Batch> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Batches");

            builder.Property(e => e.Id).HasConversion(
                    id => id.Value,
                    value => new BatchId(value))
                .ValueGeneratedNever();
            builder.Property(e => e.ProductId).HasConversion(
                    id => id.Value,
                    value => new ProductId(value));
            builder.Property(e => e.PurchaseItemId).HasConversion(
                    id => id.Value,
                    value => new PurchaseItemId(value));
            builder.Property(e => e.BrandId).HasConversion(
                    id => id.Value,
                    value => new BrandId(value));

            builder.Property(e => e.UnitCost)
                .HasColumnType("decimal(18, 2)");

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("NOW()");  // ? ????? ?? GETUTCDATE() ??? NOW()

            builder.HasOne<Product>().WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Batches_Products_ProductId");

            builder.HasOne<PurchaseItem>().WithMany()
                .HasForeignKey(d => d.PurchaseItemId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Batches_PurchaseItems_PurchaseItemId");

            builder.HasOne<Brand>().WithMany()
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}