using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
    {
        public void Configure(EntityTypeBuilder<StockMovement> builder)
        {
            builder.ToTable("StockMovements");

            builder.HasKey(e => e.Id).HasName("PK_StockMovements");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.MovementType).HasMaxLength(20);
            builder.Property(e => e.ReferenceType).HasMaxLength(50);

            builder.HasOne(d => d.Batch).WithMany(p => p.StockMovements)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.Cascade) // Changed to Cascade if Batch deleted? Or Restrict. Let's stick to existing or user preference. Usually movements are logs, shouldn't be deleted, but if Batch gone... let's say Restrict is safer for logs, but ClientSetNull was there. 
                // Wait, ClientSetNull implies nullable FK. But Id is Guid (non-nullable). So it must be Restrict or Cascade.
                // Assuming Data Integrity: Restrict.
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_StockMovements_Batches_BatchId");

            builder.HasOne(d => d.Product).WithMany(p => p.StockMovements)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_StockMovements_Products_ProductId");

            builder.HasOne(d => d.Warehouse).WithMany(p => p.StockMovements)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_StockMovements_Warehouses_WarehouseId");
        }
    }
}
