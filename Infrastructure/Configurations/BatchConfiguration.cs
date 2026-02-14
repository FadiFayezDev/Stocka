using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class BatchConfiguration : IEntityTypeConfiguration<Batch>
    {
        public void Configure(EntityTypeBuilder<Batch> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Batches");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.UnitCost).HasColumnType("decimal(18, 2)");

            builder.HasOne(d => d.Product).WithMany(p => p.Batches)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Batches_Products_ProductId");

            builder.HasOne(d => d.PurchaseItem).WithMany(p => p.Batches)
                .HasForeignKey(d => d.PurchaseItemId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Batches_PurchaseItems_PurchaseItemId");
        }
    }
}