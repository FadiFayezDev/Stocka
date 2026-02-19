using Domain.Bases;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Expenses;

public partial class Expense : IEntity<Guid>
{
    public Guid Id { get; set; }

    public Guid BrandId { get; set; }

    public Guid CategoryId { get; set; }

    public decimal Amount { get; private set; }

    public DateTime ExpenseDate { get; private set; }

    public string? Notes { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual ExpenseCategory Category { get; set; } = null!;

    private Expense() { }

    public Expense(Guid brandId, Guid categoryId, decimal amount, DateTime? expenseDate = null, string? notes = null)
    {
        Id = Guid.NewGuid();

        if (amount <= 0)
            throw new ArgumentException("Expense amount must be greater than zero.", nameof(amount));

        BrandId = brandId;
        CategoryId = categoryId;
        Amount = amount;
        ExpenseDate = expenseDate ?? DateTime.UtcNow;
        Notes = notes?.Trim();
    }

    public void UpdateAmount(decimal newAmount)
    {
        if (newAmount <= 0)
            throw new ArgumentException("Expense amount must be greater than zero.", nameof(newAmount));

        Amount = newAmount;
    }

    public void UpdateNotes(string? newNotes)
    {
        Notes = newNotes?.Trim();
    }

    public void UpdateExpenseDate(DateTime newDate)
    {
        if (newDate > DateTime.UtcNow)
            throw new ArgumentException("Expense date cannot be in the future.", nameof(newDate));

        ExpenseDate = newDate;
    }

    public Guid GetKey()
    {
        return Id;
    }

    public void SetKey(Guid id)
    {
        Id = id;
    }
}
