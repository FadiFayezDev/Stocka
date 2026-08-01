using Domain.Entities.Core;
using Domain.Primitives;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("employees");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasConversion(
                    id => id.Value,
                    value => new EmployeeId(value))
                .ValueGeneratedNever();
            builder.Property(e => e.BrandId).HasConversion(
                    id => id.Value,
                    value => new BrandId(value));
            builder.Property(e => e.BranchId).HasConversion(
                    id => id.Value,
                    value => new BranchId(value));

            builder.Property(e => e.JobTitle)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Salary)
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.HireDate)
                .HasDefaultValueSql("NOW()");

            builder.Property(e => e.IsActive)
                .HasDefaultValue(true);

            builder.HasOne<Branch>()
                .WithMany()
                .HasForeignKey(e => e.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.BrandId, e.BranchId });

            // Optional One-to-One with ApplicationUser
            builder.HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.UserId)
                .IsUnique()
                .HasFilter("\"user_id\" IS NOT NULL");
        }
    }
}
