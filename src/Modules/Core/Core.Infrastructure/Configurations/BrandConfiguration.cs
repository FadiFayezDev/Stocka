using BuildingBlocks.Application.Primitives.Identifiers;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Infrastructure.Configurations
{
    internal class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder
                .HasKey(b => b.Id);

            builder
                .Property(b => b.Id)
                .HasConversion(b => b.Value, b => new BrandId(b))
                .ValueGeneratedNever();

            builder
                .Property(b => b.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(b => b.Slug)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasMany(b => b.Memberships)
                .WithOne()
                .HasForeignKey(m => m.BrandId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata?
                .FindNavigation(nameof(Brand.Memberships))?
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
