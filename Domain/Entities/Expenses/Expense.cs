using Domain.Bases;
using Domain.Entities.Core;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Expenses;

public partial class Expense : AggregateRoot<ExpenseId>, IMultiTenantEntity, IBranchScopedEntity
{
    public BrandId BrandId { get; private set; }
    public BranchId? BranchId { get; private set; }

    public ExpenseCategoryId CategoryId { get; private set; }

    public decimal Amount { get; private set; }

    public DateTime ExpenseDate { get; private set; }

    public string? Notes { get; private set; }

    private Expense() { }

        [SetsRequiredMembers]
        public Expense(BrandId brandId, ExpenseCategoryId categoryId, decimal amount, BranchId branchId, DateTime? expenseDate = null, string? notes = null)
    {
        Id = ExpenseId.New();

        if (amount <= 0)
            throw new ArgumentException("Expense amount must be greater than zero.", nameof(amount));

        BrandId = brandId;
        BranchId = branchId;
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

    Guid IMultiTenantEntity.BrandId
    {
        get => BrandId.Value;
        set => BrandId = new BrandId(value);
    }

    Guid IBranchScopedEntity.BranchId
    {
        get => BranchId?.Value ?? Guid.Empty;
        set => BranchId = value == Guid.Empty ? null : new BranchId(value);
    }
}
