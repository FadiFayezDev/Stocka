using Domain.Entities.Orders;
using Domain.Entities.Products;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("order_items");

            builder.HasKey(e => e.Id).HasName("PK_OrderItems");

            builder.Property(e => e.Id).HasConversion(
                    id => id.Value,
                    value => new OrderItemId(value))
                .ValueGeneratedNever();
            builder.Property(e => e.OrderId).HasConversion(
                    id => id.Value,
                    value => new OrderId(value));
            builder.Property(e => e.ProductId).HasConversion(
                    id => id.Value,
                    value => new ProductId(value));
            builder.Property(e => e.BatchId).HasConversion(
                    id => id.Value,
                    value => new BatchId(value));
            builder.Property(e => e.Quantity)
                .IsRequired();
            builder.Property(e => e.CostPrice)
                .HasColumnType("decimal(18, 2)");
            builder.Property(e => e.UnitPrice)
                .HasColumnType("decimal(18, 2)");

            builder.HasOne<Batch>().WithMany()
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OrderItems_Batches_BatchId");

            builder.HasOne<Product>().WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OrderItems_Products_ProductId");

            builder.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OrderItems_Orders_OrderId");

            //builder.HasOne(d => d.Brand).WithMany()
            //    .HasForeignKey(d => d.BrandId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
