using Domain.Entities.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class JournalEntryConfiguration : IEntityTypeConfiguration<JournalEntry>
    {
        public void Configure(EntityTypeBuilder<JournalEntry> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_JournalEntries");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.EntryDate)
                .HasDefaultValueSql("NOW()"); // Changed from GETUTCDATE() to NOW()
            builder.Property(e => e.Description)
                .HasMaxLength(500);

            builder.HasOne(d => d.Brand).WithMany(p => p.JournalEntries)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_JournalEntries_Brands_BrandId");

            // Configure JournalEntryLines collection
            builder.HasMany(d => d.JournalEntryLines)
                .WithOne(d => d.JournalEntry)
                .HasForeignKey(d => d.JournalEntryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(d => d.JournalEntryLines).AutoInclude();
        }
    }
}
