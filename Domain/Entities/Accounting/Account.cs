using Domain.Bases;
using Domain.Entities.Core;
using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Accounting;

public partial class Account : IEntity<Guid>
{
    public Guid Id { get; set; }

    public Guid BrandId { get; set; }

    public string Name { get; set; } = null!;

    public AccountType Type { get; set; }

    private readonly List<JournalEntryLine> _journalEntryLines = new();
    public virtual Brand Brand { get; set; } = null!;
    public virtual ICollection<JournalEntryLine> JournalEntryLines => _journalEntryLines.AsReadOnly();

    private Account() { }

    public Account(Guid brandId, string name, AccountType type)
    {
        Id = Guid.NewGuid();

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

    public Guid GetKey() => Id;

    public void SetKey(Guid key) => Id = key;
}
