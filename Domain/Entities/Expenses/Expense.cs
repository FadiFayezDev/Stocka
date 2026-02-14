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

    public decimal Amount { get; set; }

    public DateTime ExpenseDate { get; set; }

    public string? Notes { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual ExpenseCategory Category { get; set; } = null!;

    public Guid GetKey()
    {
        return Id;
    }

    public void SetKey(Guid id)
    {
        Id = id;
    }
}
