
using BuildingBlocks.Application.Primitives.Identifiers;
using Inventory.Domain.Entities;
using Inventory.Primitives.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Configurations
{
    public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
    {
        public void Configure(EntityTypeBuilder<StockMovement> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_StockMovements");

            builder
                .Property(e => e.Id).HasConversion(
                    id => id.Value,
                    value => new StockMovementId(value))
                .ValueGeneratedNever();

            builder
                .Property(e => e.ProductId)
                .IsRequired();

            builder
                .Property(e => e.BatchId).HasConversion(
                    id => id.Value,
                    value => new BatchId(value));

            builder
                .Property(e => e.WarehouseId).HasConversion(
                    id => id.Value,
                    value => new WarehouseId(value));

            builder
                .Property(e => e.BrandId).HasConversion(
                    id => id.Value,
                    value => new BrandId(value));

            builder
                .Property(e => e.Quantity)
                .IsRequired();

            builder
                .Property(e => e.MovementType)
                .IsRequired();

            builder
                .Property(e => e.CreatedAt)
                .IsRequired();

            builder
                .Property(e => e.Reason)
                .HasColumnType("NVARCHAR(300)")
                .IsRequired(false);

            // Report-supporting indexes: brand-scoped date scans and warehouse/product histories.
            builder.HasIndex(e => new { e.BrandId, e.CreatedAt });
            builder.HasIndex(e => new { e.WarehouseId, e.CreatedAt });
            builder.HasIndex(e => new { e.BrandId, e.ProductId });

            // Domain invariant persisted at the database level: Quantity > 0.
            builder.ToTable("StockMovements", t =>
                t.HasCheckConstraint("CK_StockMovements_Quantity_Positive", "[Quantity] > 0"));

            builder
                .HasOne<Batch>().WithMany()
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_StockMovements_Batches_BatchId");

            //builder.HasOne<Product>().WithMany()
            //    .HasForeignKey(d => d.ProductId)
            //    .OnDelete(DeleteBehavior.Restrict)
            //    .HasConstraintName("FK_StockMovements_Products_ProductId");

            builder
                .HasOne<Warehouse>().WithMany()
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_StockMovements_Warehouses_WarehouseId");

            //builder.HasOne<Brand>().WithMany()
            //    .HasForeignKey(d => d.BrandId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
