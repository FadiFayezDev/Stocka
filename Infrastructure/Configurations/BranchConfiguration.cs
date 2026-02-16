using Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Branches");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasOne(d => d.Brand).WithMany(p => p.Branches)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Branches_Brands_BrandId");

            // Configure Warehouses collection
            builder.HasMany(d => d.Warehouses)
                .WithOne(d => d.Branch)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(d => d.Warehouses).AutoInclude();
        }
    }
}
