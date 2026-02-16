using Domain.Entities.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ExpenseCategoryConfiguration : IEntityTypeConfiguration<ExpenseCategory>
    {
        public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_ExpenseCategories");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasOne(d => d.Brand).WithMany(p => p.ExpenseCategories)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ExpenseCategories_Brands_BrandId");

            // Configure Expenses collection
            builder.HasMany(d => d.Expenses)
                .WithOne(d => d.Category)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(d => d.Expenses).AutoInclude();
        }
    }
}
