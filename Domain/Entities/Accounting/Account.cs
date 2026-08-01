using Domain.Bases;
using Domain.Entities.Core;
using Domain.Enums;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Accounting;

public partial class Account : AggregateRoot<AccountId>, IMultiTenantEntity
{
    public BrandId BrandId { get; private set; }

    public string Name { get; private set; } = null!;

    public AccountType Type { get; private set; }

    private readonly List<JournalEntryLine> _journalEntryLines = new();
    public virtual Brand Brand { get; private set; } = null!;
    public virtual ICollection<JournalEntryLine> JournalEntryLines => _journalEntryLines.AsReadOnly();

    private Account() { }

        [SetsRequiredMembers]
        public Account(BrandId brandId, string name, AccountType type)
    {
        Id = AccountId.New();

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Account name cannot be empty.", nameof(name));

        BrandId = brandId;
        Name = name.Trim();
        Type = type;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Account name cannot be empty.", nameof(newName));

        Name = newName.Trim();
    }

    public void UpdateType(AccountType newType)
    {
        Type = newType;
    }

    public void AddJournalEntryLine(JournalEntryLine line)
    {
        if (line == null)
            throw new ArgumentNullException(nameof(line));

        if (line.AccountId != Id)
            throw new ArgumentException("Journal entry line does not belong to this account.");

        if (_journalEntryLines.Any(l => l.Id == line.Id))
            throw new InvalidOperationException("Journal entry line already added.");

        _journalEntryLines.Add(line);
    }

    Guid IMultiTenantEntity.BrandId
    {
        get => BrandId.Value;
        set => BrandId = new BrandId(value);
    }
}
