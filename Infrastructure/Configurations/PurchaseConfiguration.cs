using Domain.Entities.Purchasing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Purchases");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.PurchaseDate)
                .HasDefaultValueSql("NOW()");  // ? ????? ?? GETUTCDATE() ??? NOW()
            builder.Property(e => e.TotalAmount)
                .HasColumnType("decimal(18, 2)")
                .HasDefaultValue(0);

            builder.HasOne(d => d.Brand).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Purchases_Brands_BrandId");

            builder.HasOne(d => d.Supplier).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Purchases_Suppliers_SupplierId");

            // Configure PurchaseItems collection
            builder.HasMany(d => d.PurchaseItems)
                .WithOne(d => d.Purchase)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(d => d.PurchaseItems).AutoInclude();
        }
    }
}
