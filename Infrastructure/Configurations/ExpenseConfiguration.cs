using Domain.Entities.Expenses;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Expenses");

            builder.Property(e => e.Id).HasConversion(
                    id => id.Value,
                    value => new ExpenseId(value))
                .ValueGeneratedNever();
            builder.Property(e => e.BrandId).HasConversion(
                    id => id.Value,
                    value => new BrandId(value));
            builder.Property(e => e.BranchId).HasConversion(
                    id => id.Value.Value,
                    value => new BranchId(value));
            builder.Property(e => e.CategoryId).HasConversion(
                    id => id.Value,
                    value => new ExpenseCategoryId(value));
            builder.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)");
            builder.Property(e => e.ExpenseDate)
                .HasDefaultValueSql("NOW()");
            builder.Property(e => e.Notes)
                .HasMaxLength(500);

            builder.HasOne(d => d.Brand).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Expenses_Brands_BrandId");

            builder.HasOne(d => d.Branch).WithMany()
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Expenses_Branches_BranchId");

            builder.HasOne(d => d.Category).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Expenses_ExpenseCategories_CategoryId");

            builder.HasIndex(e => new { e.BrandId, e.BranchId, e.ExpenseDate });
        }
    }
}
