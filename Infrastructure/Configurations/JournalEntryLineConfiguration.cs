using Domain.Entities.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class JournalEntryLineConfiguration : IEntityTypeConfiguration<JournalEntryLine>
    {
        public void Configure(EntityTypeBuilder<JournalEntryLine> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_JournalEntryLines");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Credit)
                .HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Debit)
                .HasColumnType("decimal(18, 2)");

            builder.HasOne(d => d.Account).WithMany(p => p.JournalEntryLines)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_JournalEntryLines_Accounts_AccountId");

            builder.HasOne(d => d.JournalEntry).WithMany(p => p.JournalEntryLines)
                .HasForeignKey(d => d.JournalEntryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_JournalEntryLines_JournalEntries_JournalEntryId");

            builder.HasOne(d => d.Brand).WithMany()
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index for queries
            builder.HasIndex(d => d.JournalEntryId);
            builder.HasIndex(d => d.AccountId);
        }
    }
}