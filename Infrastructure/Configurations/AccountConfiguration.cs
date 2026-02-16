using Domain.Entities.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Accounts");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(e => e.Type)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.HasOne(d => d.Brand).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Accounts_Brands_BrandId");

            // Configure JournalEntryLines collection
            builder.HasMany(d => d.JournalEntryLines)
                .WithOne(d => d.Account)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(d => d.JournalEntryLines).AutoInclude();
        }
    }
}