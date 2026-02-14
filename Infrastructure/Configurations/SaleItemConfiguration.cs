using Domain.Entities.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SaleItems");

            builder.HasKey(e => e.Id).HasName("PK_SaleItems");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.CostPrice).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            builder.HasOne(d => d.Batch).WithMany(p => p.SaleItems)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleItems_Batches_BatchId");

            builder.HasOne(d => d.Product).WithMany(p => p.SaleItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleItems_Products_ProductId");

            builder.HasOne(d => d.Sale).WithMany(p => p.SaleItems)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.Cascade) // Cascade delete lines when sale is deleted
                .HasConstraintName("FK_SaleItems_Sales_SaleId");
        }
    }
}
