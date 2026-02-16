using Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(e => e.Id).HasName("PK_OrderItems");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Quantity)
                .IsRequired();
            builder.Property(e => e.CostPrice)
                .HasColumnType("decimal(18, 2)");
            builder.Property(e => e.UnitPrice)
                .HasColumnType("decimal(18, 2)");

            builder.HasOne(d => d.Batch).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OrderItems_Batches_BatchId");

            builder.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OrderItems_Products_ProductId");

            builder.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OrderItems_Orders_OrderId");

            builder.HasOne(d => d.Brand).WithMany()
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
