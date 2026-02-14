using Domain.Bases;
using Domain.Entities.Products;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Core;

public partial class Branch : IEntity<Guid>
{
    public Guid Id { get; set; }

    public Guid BrandId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();

    public Guid GetKey()
    {
        return Id;
    }

    public void SetKey(Guid id)
    {
        Id = id;
    }
}
