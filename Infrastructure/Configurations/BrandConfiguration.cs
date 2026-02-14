using Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Brands");

            builder.HasIndex(e => e.Slug, "UQ_Brands_Slug").IsUnique();

            builder.HasMany(b => b.Memberships)
                .WithOne()
                .HasForeignKey(m => m.BrandId)
                .OnDelete(DeleteBehavior.Cascade);
        
            builder.Metadata?
                .FindNavigation(nameof(Brand.Memberships))?
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            builder.Property(e => e.Name).HasMaxLength(200);
            builder.Property(e => e.Slug).HasMaxLength(100);
        }
    }
}
