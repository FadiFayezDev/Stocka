using Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Orders");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.OrderDate)
                .HasDefaultValueSql("NOW()");  // ? ????? ?? GETUTCDATE() ??? NOW()
            builder.Property(e => e.Status)
                .HasConversion<string>()
                .HasMaxLength(50);
            builder.Property(e => e.TotalAmount)
                .HasColumnType("decimal(18, 2)")
                .HasDefaultValue(0);

            builder.HasOne(d => d.Brand).WithMany(p => p.Orders)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Orders_Brands_BrandId");

            builder.HasOne(d => d.Employee).WithMany()
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Orders_Employees_EmployeeId");

            builder.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Orders_Customers_CustomerId");

            // Configure OrderItems collection
            builder.HasMany(d => d.OrderItems)
                .WithOne(d => d.Order)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(d => d.OrderItems).AutoInclude();
        }
    }
}
