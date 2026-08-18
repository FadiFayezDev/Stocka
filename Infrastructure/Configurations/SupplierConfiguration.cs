using Domain.Entities.Core;
using Domain.Entities.Purchasing;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("suppliers");

            builder.HasKey(e => e.Id).HasName("PK_Suppliers");

            builder.Property(e => e.Id).HasConversion(
                    id => id.Value,
                    value => new SupplierId(value))
                .ValueGeneratedNever();
            builder.Property(e => e.BrandId).HasConversion(
                    id => id.Value,
                    value => new BrandId(value));
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(e => e.Phone)
                .HasMaxLength(20);
            builder.Property(e => e.Email)
                .HasMaxLength(100);
            builder.Property(e => e.Address)
                .HasMaxLength(500);

            builder.HasOne<Brand>().WithMany()
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Suppliers_Brands_BrandId");
        }
    }
}
