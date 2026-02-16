using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable("Warehouses");

            builder.HasKey(e => e.Id).HasName("PK_Warehouses");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(e => e.Type)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.HasOne(d => d.Branch).WithMany(p => p.Warehouses)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Warehouses_Branches_BranchId");

            builder.HasOne(d => d.Brand).WithMany()
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure StockMovements collection
            builder.HasMany(d => d.StockMovements)
                .WithOne(d => d.Warehouse)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure WarehouseBatches collection
            builder.HasMany(d => d.WarehouseBatches)
                .WithOne(d => d.Warehouse)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(d => d.WarehouseBatches).AutoInclude();
        }
    }
}
