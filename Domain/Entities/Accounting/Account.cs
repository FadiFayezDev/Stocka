using Domain.Bases;
using Domain.Entities.Core;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Accounting;

public partial class Account : IEntity<Guid>
{
    public Guid GetKey() => Id;

    public void SetKey(Guid key) => Id = key;


    public Guid Id { get; set; }

    public Guid BrandId { get; set; }

    public string Name { get; set; } = null!;

    public AccountType Type { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
}
