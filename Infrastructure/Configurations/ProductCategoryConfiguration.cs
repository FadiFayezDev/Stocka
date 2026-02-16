using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_ProductCategories");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasOne(d => d.Brand).WithMany(p => p.ProductCategories)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ProductCategories_Brands_BrandId");

            // Configure Products collection
            builder.HasMany(d => d.Products)
                .WithOne(d => d.Category)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(d => d.Products).AutoInclude();
        }
    }
}