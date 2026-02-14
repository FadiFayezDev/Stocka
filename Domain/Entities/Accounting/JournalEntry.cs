using Domain.Bases;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Accounting;

public partial class JournalEntry : IEntity<Guid>
{
    public Guid GetKey() => Id;

    public void SetKey(Guid key) => Id = key;

    public Guid Id { get; set; }

    public Guid BrandId { get; set; }

    public DateTime EntryDate { get; set; }

    public string? Description { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
}
