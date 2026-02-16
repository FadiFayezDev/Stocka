using Domain.Entities.Core;
using Domain.ValueObjects;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class BrandMembershipConfiguration : IEntityTypeConfiguration<BrandMembership>
    {
        public void Configure(EntityTypeBuilder<BrandMembership> builder)
        {
            builder.ToTable("BrandMemberships");

            builder.HasKey(m => new { m.BrandId, m.UserId });

            builder.Property(m => m.Role)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne<Brand>()                     // 👑 العلاقة
                   .WithMany(b => b.Memberships)
                   .HasForeignKey(m => m.BrandId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}