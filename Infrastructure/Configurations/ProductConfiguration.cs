using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Products");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Barcode).HasMaxLength(100);
            builder.Property(e => e.Name).HasMaxLength(200);

            builder.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull) // Product belongs to Brand. If Brand deleted? Usually cascade, but user asked for "Restrict" or consistent. Usually Brand deletion is huge. Let's keep ClientSetNull or Restrict. Actually, Product is master data. Restrict is safer.
                .HasConstraintName("FK_Products_Brands_BrandId");

            builder.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict) // Category deletion shouldn't delete products silently
                .HasConstraintName("FK_Products_ProductCategories_CategoryId");
        }
    }
}
