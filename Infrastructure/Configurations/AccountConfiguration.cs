using Domain.Entities.Accounting;
using Domain.Entities.Core;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Accounts");

            builder.Property(e => e.Id).HasConversion(
                    id => id.Value,
                    value => new AccountId(value))
                .ValueGeneratedNever();
            builder.Property(e => e.BrandId).HasConversion(
                    id => id.Value,
                    value => new BrandId(value));

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(e => e.Type)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.HasOne<Brand>().WithMany()
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Accounts_Brands_BrandId");
        }
    }
}