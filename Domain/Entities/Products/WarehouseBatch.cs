using Domain.Bases;
using Domain.Entities.Core;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Products;

public partial class WarehouseBatch : IEntity<Guid>
{
    public Guid GetKey() => Id;

    public void SetKey(Guid key) => Id = key;

    public Guid Id { get; set; }

    public Guid WarehouseId { get; set; }
    public Guid BatchId { get; set; }
    public int Quantity { get; set; }

    public Guid BrandId { get; set; }

    public virtual Warehouse Warehouse { get; set; } = null!;
    public virtual Batch Batch { get; set; } = null!;

    public virtual Brand Brand { get; set; } = null!;
}