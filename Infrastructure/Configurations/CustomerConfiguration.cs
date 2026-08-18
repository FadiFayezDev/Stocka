using Domain.Entities.Core;
using Domain.Primitives;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("customers");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                    id => id.Value,
                    value => new CustomerId(value))
                .ValueGeneratedNever();
            builder.Property(c => c.BrandId).HasConversion(
                    id => id.Value,
                    value => new BrandId(value));

            builder.Property(c => c.LoyaltyPoints)
                .HasDefaultValue(0);

            builder.HasOne<Brand>()
                .WithMany()
                .HasForeignKey(c => c.BrandId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional One-to-One with ApplicationUser
            builder.HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<Customer>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => c.UserId)
                .IsUnique()
                .HasFilter("\"user_id\" IS NOT NULL");
        }
    }
}