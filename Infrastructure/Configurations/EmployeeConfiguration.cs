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

            builder.Property(e => e.HireDate)
                .HasDefaultValueSql("NOW()");  // ✅ تغيير من GETUTCDATE() إلى NOW()

            builder.Property(e => e.IsActive)
                .HasDefaultValue(true);

            // Optional One-to-One with ApplicationUser
            builder.HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.UserId)
                .IsUnique()
                .HasFilter("\"UserId\" IS NOT NULL");  // ✅ تغيير من [UserId] إلى "UserId" (PostgreSQL syntax)
        }
    }
}
