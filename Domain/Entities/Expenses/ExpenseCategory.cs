using Domain.Bases;
using Domain.Entities.Core;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Domain.Entities.Expenses;

public partial class ExpenseCategory : AggregateRoot<ExpenseCategoryId>, IMultiTenantEntity
{
    public BrandId BrandId { get; private set; }

    public string Name { get; private set; } = null!;

    private ExpenseCategory() { }

        [SetsRequiredMembers]
        public ExpenseCategory(BrandId brandId, string name)
    {
        Id = ExpenseCategoryId.New();

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Expense category name cannot be empty.", nameof(name));

        BrandId = brandId;
        Name = name.Trim();
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Expense category name cannot be empty.", nameof(newName));

        Name = newName.Trim();
    }

    Guid IMultiTenantEntity.BrandId
    {
        get => BrandId.Value;
        set => BrandId = new BrandId(value);
    }
}
