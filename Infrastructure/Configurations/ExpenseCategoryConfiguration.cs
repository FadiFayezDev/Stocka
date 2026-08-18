using Domain.Entities.Core;
using Domain.Entities.Expenses;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ExpenseCategoryConfiguration : IEntityTypeConfiguration<ExpenseCategory>
    {
        public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_ExpenseCategories");

            builder.Property(e => e.Id).HasConversion(
                    id => id.Value,
                    value => new ExpenseCategoryId(value))
                .ValueGeneratedNever();
            builder.Property(e => e.BrandId).HasConversion(
                    id => id.Value,
                    value => new BrandId(value));
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasOne<Brand>().WithMany()
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ExpenseCategories_Brands_BrandId");
        }
    }
}
