using Domain.Entities.Purchasing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Suppliers");

            builder.HasKey(e => e.Id).HasName("PK_Suppliers");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(e => e.Phone)
                .HasMaxLength(20);
            builder.Property(e => e.Email)
                .HasMaxLength(100);
            builder.Property(e => e.Address)
                .HasMaxLength(500);

            builder.HasOne(d => d.Brand).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Suppliers_Brands_BrandId");

            // Configure Purchases collection
            builder.HasMany(d => d.Purchases)
                .WithOne(d => d.Supplier)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(d => d.Purchases).AutoInclude();
        }
    }
}
