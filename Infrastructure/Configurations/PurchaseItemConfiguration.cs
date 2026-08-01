using Domain.Entities.Purchasing;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class PurchaseItemConfiguration : IEntityTypeConfiguration<PurchaseItem>
    {
        public void Configure(EntityTypeBuilder<PurchaseItem> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_PurchaseItems");

            builder.Property(e => e.Id).HasConversion(
                    id => id.Value,
                    value => new PurchaseItemId(value))
                .ValueGeneratedNever();
            builder.Property(e => e.PurchaseId).HasConversion(
                    id => id.Value,
                    value => new PurchaseId(value));
            builder.Property(e => e.ProductId).HasConversion(
                    id => id.Value,
                    value => new ProductId(value));
            builder.Property(e => e.Quantity)
                .IsRequired();
            builder.Property(e => e.UnitCost)
                .HasColumnType("decimal(18, 2)");

            builder.HasOne(d => d.Purchase).WithMany(p => p.PurchaseItems)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PurchaseItems_Purchases_PurchaseId");

            builder.HasOne(d => d.Product).WithMany(p => p.PurchaseItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PurchaseItems_Products_ProductId");

            // Configure Batches collection
            builder.HasMany(d => d.Batches)
                .WithOne(d => d.PurchaseItem)
                .HasForeignKey(d => d.PurchaseItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(d => d.Batches).AutoInclude();
        }
    }
}