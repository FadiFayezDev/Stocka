using Domain.Bases;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities.Accounting;

public partial class JournalEntry : IEntity<Guid>
{
    public Guid Id { get; set; }

    public Guid BrandId { get; set; }

    public DateTime EntryDate { get; set; }

    public string? Description { get; set; }

    private readonly List<JournalEntryLine> _journalEntryLines = new();

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<JournalEntryLine> JournalEntryLines => _journalEntryLines.AsReadOnly();

    private JournalEntry() { }

    public JournalEntry(Guid brandId, DateTime? entryDate = null, string? description = null)
    {
        Id = Guid.NewGuid();

        BrandId = brandId;
        EntryDate = entryDate ?? DateTime.UtcNow;
        Description = description?.Trim();
    }

    public void UpdateDescription(string? newDescription)
    {
        Description = newDescription?.Trim();
    }

    public void AddJournalEntryLine(JournalEntryLine line)
    {
        if (line == null)
            throw new ArgumentNullException(nameof(line));

        if (line.JournalEntryId != Id)
            throw new ArgumentException("Journal entry line does not belong to this journal entry.");

        if (_journalEntryLines.Any(l => l.Id == line.Id))
            throw new InvalidOperationException("Journal entry line already added.");

        ValidateBalance(line);
        _journalEntryLines.Add(line);
    }

    public void RemoveJournalEntryLine(Guid lineId)
    {
        var line = _journalEntryLines.FirstOrDefault(l => l.Id == lineId);
        if (line == null)
            throw new ArgumentException("Journal entry line not found.");

        _journalEntryLines.Remove(line);
    }

    private void ValidateBalance(JournalEntryLine newLine)
    {
        decimal totalDebit = _journalEntryLines.Sum(l => l.Debit) + newLine.Debit;
        decimal totalCredit = _journalEntryLines.Sum(l => l.Credit) + newLine.Credit;

        if (totalDebit != totalCredit && _journalEntryLines.Count > 0)
            throw new InvalidOperationException("Journal entry must balance (Total Debit = Total Credit).");
    }

    public decimal GetTotalDebit => _journalEntryLines.Sum(l => l.Debit);
    public decimal GetTotalCredit => _journalEntryLines.Sum(l => l.Credit);
    public bool IsBalanced => GetTotalDebit == GetTotalCredit;

    public Guid GetKey() => Id;

    public void SetKey(Guid key) => Id = key;
}
