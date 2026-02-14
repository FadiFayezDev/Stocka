using Domain.Entities.Purchasing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class PurchaseItemConfiguration : IEntityTypeConfiguration<PurchaseItem>
    {
        public void Configure(EntityTypeBuilder<PurchaseItem> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_PurchaseItems");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.UnitCost).HasColumnType("decimal(18, 2)");

            builder.HasOne(d => d.Purchase).WithMany(p => p.PurchaseItems)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade) // Purchase deletion cascades to items
                .HasConstraintName("FK_PurchaseItems_Purchases_PurchaseId");

            builder.HasOne(d => d.Product).WithMany(p => p.PurchaseItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict) // Don't delete product if used in purchase
                .HasConstraintName("FK_PurchaseItems_Products_ProductId");
        }
    }
}