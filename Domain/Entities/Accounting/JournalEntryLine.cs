using Domain.Bases;
using Domain.Entities.Core;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Accounting;

public partial class JournalEntryLine : AggregateRoot<JournalEntryLineId>, IMultiTenantEntity
{
    public JournalEntryId JournalEntryId { get; private set; }

    public AccountId AccountId { get; private set; }

    public BrandId BrandId { get; private set; }

    public decimal Debit { get; private set; }

    public decimal Credit { get; private set; }

    public virtual Account Account { get; private set; } = null!;

    public virtual JournalEntry JournalEntry { get; private set; } = null!; 
    
    public virtual Brand Brand { get; private set; } = null!;

    private JournalEntryLine() { }

        [SetsRequiredMembers]
        public JournalEntryLine(JournalEntryId journalEntryId, AccountId accountId, BrandId brandId, decimal debit, decimal credit)
    {
        Id = JournalEntryLineId.New();

        if (debit < 0 || credit < 0)
            throw new ArgumentException("Debit and Credit must be non-negative.");

        if (debit == 0 && credit == 0)
            throw new ArgumentException("At least one of Debit or Credit must be greater than zero.");

        if (debit > 0 && credit > 0)
            throw new ArgumentException("Both Debit and Credit cannot be greater than zero at the same time.");

        JournalEntryId = journalEntryId;
        AccountId = accountId;
        BrandId = brandId;
        Debit = debit;
        Credit = credit;
    }

    public void UpdateDebit(decimal newDebit)
    {
        if (newDebit < 0)
            throw new ArgumentException("Debit must be non-negative.", nameof(newDebit));

        if (newDebit > 0 && Credit > 0)
            throw new ArgumentException("Both Debit and Credit cannot be greater than zero at the same time.");

        Debit = newDebit;
    }

    public void UpdateCredit(decimal newCredit)
    {
        if (newCredit < 0)
            throw new ArgumentException("Credit must be non-negative.", nameof(newCredit));

        if (newCredit > 0 && Debit > 0)
            throw new ArgumentException("Both Debit and Credit cannot be greater than zero at the same time.");

        Credit = newCredit;
    }

    public decimal GetAmount => Math.Max(Debit, Credit);
    public bool IsDebit => Debit > 0;
    public bool IsCredit => Credit > 0;

    Guid IMultiTenantEntity.BrandId
    {
        get => BrandId.Value;
        set => BrandId = new BrandId(value);
    }
}
