using Domain.Bases;
using Domain.Entities.Core;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Domain.Entities.Accounting;

public partial class JournalEntry : AggregateRoot<JournalEntryId>, IMultiTenantEntity
{
    public BrandId BrandId { get; private set; }

    public DateTime EntryDate { get; private set; }

    public string? Description { get; private set; }

    private readonly List<JournalEntryLine> _journalEntryLines = new();

    public virtual ICollection<JournalEntryLine> JournalEntryLines => _journalEntryLines.AsReadOnly();

    private JournalEntry() { }

        [SetsRequiredMembers]
        public JournalEntry(BrandId brandId, DateTime? entryDate = null, string? description = null)
    {
        Id = JournalEntryId.New();

        BrandId = brandId;
        EntryDate = entryDate ?? DateTime.UtcNow;
        Description = description?.Trim();
    }

    public void UpdateDescription(string? newDescription)
    {
        Description = newDescription?.Trim();
    }

    public void UpdateEntryDate(DateTime newDate)
    {
        if (newDate > DateTime.UtcNow)
            throw new ArgumentException("Entry date cannot be in the future.", nameof(newDate));

        EntryDate = newDate;
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

    public JournalEntryLine AddJournalEntryLine(AccountId accountId, BrandId brandId, decimal debit, decimal credit)
    {
        var line = new JournalEntryLine(Id, accountId, brandId, debit, credit);
        AddJournalEntryLine(line);
        return line;
    }

    public void RemoveJournalEntryLine(JournalEntryLineId lineId)
    {
        var line = _journalEntryLines.FirstOrDefault(l => l.Id == lineId);
        if (line == null)
            throw new ArgumentException("Journal entry line not found.");

        _journalEntryLines.Remove(line);
    }

    public void UpdateJournalEntryLine(JournalEntryLineId lineId, decimal debit, decimal credit)
    {
        var line = _journalEntryLines.FirstOrDefault(l => l.Id == lineId);
        if (line == null)
            throw new ArgumentException("Journal entry line not found.");

        line.UpdateDebit(debit);
        line.UpdateCredit(credit);
        ValidateBalancedState();
    }

    private void ValidateBalance(JournalEntryLine newLine)
    {
        decimal totalDebit = _journalEntryLines.Sum(l => l.Debit) + newLine.Debit;
        decimal totalCredit = _journalEntryLines.Sum(l => l.Credit) + newLine.Credit;

        if (totalDebit != totalCredit && _journalEntryLines.Count > 0)
            throw new InvalidOperationException("Journal entry must balance (Total Debit = Total Credit).");
    }

    private void ValidateBalancedState()
    {
        decimal totalDebit = _journalEntryLines.Sum(l => l.Debit);
        decimal totalCredit = _journalEntryLines.Sum(l => l.Credit);

        if (totalDebit != totalCredit)
            throw new InvalidOperationException("Journal entry must balance (Total Debit = Total Credit).");
    }

    public decimal GetTotalDebit => _journalEntryLines.Sum(l => l.Debit);
    public decimal GetTotalCredit => _journalEntryLines.Sum(l => l.Credit);
    public bool IsBalanced => GetTotalDebit == GetTotalCredit;

    Guid IMultiTenantEntity.BrandId
    {
        get => BrandId.Value;
        set => BrandId = new BrandId(value);
    }
}
