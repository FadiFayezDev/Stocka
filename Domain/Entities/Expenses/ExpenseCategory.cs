using Domain.Bases;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Expenses;

public partial class ExpenseCategory : IEntity<Guid>
{
    public Guid Id { get; set; }

    public Guid BrandId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public Guid GetKey()
    {
        return Id;
    }

    public void SetKey(Guid id)
    {
        Id = id;
    }
}
