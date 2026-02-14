using Domain.Entities.Core;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.JobTitle)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Salary)
                .HasColumnType("decimal(18,2)");

            // ✅ Optional One-to-One with Identity
            builder.HasOne<ApplicationUser>()
                .WithOne() // 👑 no navigation back (cleaner)
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Unique UserId لو موجود
            builder.HasIndex(e => e.UserId)
                .IsUnique()
                .HasFilter("[UserId] IS NOT NULL"); // مهم جدًا

        }
    }
}
