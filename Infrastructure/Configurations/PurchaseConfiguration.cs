using Domain.Entities.Core;
using Domain.Entities.Purchasing;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Purchases");

            builder.Property(e => e.Id).HasConversion(
                    id => id.Value,
                    value => new PurchaseId(value))
                .ValueGeneratedNever();
            builder.Property(e => e.BrandId).HasConversion(
                    id => id.Value,
                    value => new BrandId(value));
            builder.Property(e => e.BranchId).HasConversion(
                    id => id.Value.Value,
                    value => new BranchId(value));
            builder.Property(e => e.SupplierId).HasConversion(
                    id => id.Value,
                    value => new SupplierId(value));
            builder.Property(e => e.PurchaseDate)
                .HasDefaultValueSql("NOW()");  // ? ????? ?? GETUTCDATE() ??? NOW()
            builder.Property(e => e.TotalAmount)
                .HasColumnType("decimal(18, 2)")
                .HasDefaultValue(0);

            builder.HasOne<Brand>().WithMany()
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Purchases_Brands_BrandId");

            builder.HasOne<Branch>().WithMany()
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Purchases_Branches_BranchId");

            builder.HasOne<Supplier>().WithMany()
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Purchases_Suppliers_SupplierId");

            // Configure PurchaseItems collection
            builder.HasMany(d => d.PurchaseItems)
                .WithOne(d => d.Purchase)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.BrandId, e.BranchId, e.PurchaseDate });

            builder.Navigation(d => d.PurchaseItems).AutoInclude();
        }
    }
}
