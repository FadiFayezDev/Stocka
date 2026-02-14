using Domain.Entities.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class JournalEntryConfiguration : IEntityTypeConfiguration<JournalEntry>
    {
        public void Configure(EntityTypeBuilder<JournalEntry> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_JournalEntries");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Description).HasMaxLength(500);

            builder.HasOne(d => d.Brand).WithMany(p => p.JournalEntries)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JournalEntries_Brands_BrandId");
        }
    }
}
