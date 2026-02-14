using Domain.Bases;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Accounting;

public partial class JournalEntryLine : IEntity<Guid>
{
    public Guid GetKey() => Id;

    public void SetKey(Guid key) => Id = key;

    public Guid Id { get; set; }

    public Guid JournalEntryId { get; set; }

    public Guid AccountId { get; set; }

    public Guid BrandId { get; set; }

    public decimal Debit { get; set; }

    public decimal Credit { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual JournalEntry JournalEntry { get; set; } = null!; 
    
    public virtual Brand Brand { get; set; } = null!;
}
