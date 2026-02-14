using Domain.Entities.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class JournalEntryLineConfiguration : IEntityTypeConfiguration<JournalEntryLine>
    {
        public void Configure(EntityTypeBuilder<JournalEntryLine> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_JournalEntryLines");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Credit).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Debit).HasColumnType("decimal(18, 2)");

            builder.HasOne(d => d.Account).WithMany(p => p.JournalEntryLines)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JournalEntryLines_Accounts_AccountId");

            builder.HasOne(d => d.JournalEntry).WithMany(p => p.JournalEntryLines)
                .HasForeignKey(d => d.JournalEntryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JournalEntryLines_JournalEntries_JournalEntryId");
        }
    }
}