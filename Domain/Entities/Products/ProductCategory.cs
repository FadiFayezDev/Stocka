using Domain.Bases;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Products;

public partial class ProductCategory : IEntity<Guid>
{
    public Guid GetKey() => Id;

    public void SetKey(Guid key) => Id = key;

    public Guid Id { get; set; }

    public Guid BrandId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>(); 
}
