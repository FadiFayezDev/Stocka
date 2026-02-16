using Domain.Bases;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Accounting;

public partial class JournalEntryLine : IEntity<Guid>
{
    public Guid Id { get; set; }

    public Guid JournalEntryId { get; set; }

    public Guid AccountId { get; set; }

    public Guid BrandId { get; set; }

    public decimal Debit { get; set; }

    public decimal Credit { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual JournalEntry JournalEntry { get; set; } = null!; 
    
    public virtual Brand Brand { get; set; } = null!;

    private JournalEntryLine() { }

    public JournalEntryLine(Guid journalEntryId, Guid accountId, Guid brandId, decimal debit, decimal credit)
    {
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

    public Guid GetKey() => Id;

    public void SetKey(Guid key) => Id = key;
}
