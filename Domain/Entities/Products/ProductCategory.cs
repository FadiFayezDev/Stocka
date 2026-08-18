using Domain.Bases;
using Domain.Entities.Core;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Domain.Entities.Products;

public partial class ProductCategory : AggregateRoot<ProductCategoryId>, IMultiTenantEntity
{
    public BrandId BrandId { get; private set; }

    public string Name { get; private set; } = null!;

    public bool IsActive { get; private set; }

    private ProductCategory() { }

        [SetsRequiredMembers]
        public ProductCategory(BrandId brandId, string name)
    {
        Id = ProductCategoryId.New();

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty.", nameof(name));

        BrandId = brandId;
        Name = name.Trim();
        IsActive = true;
    }

    public void UpdateActive(bool value) => IsActive = value;

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Category name cannot be empty.", nameof(newName));

        Name = newName.Trim();
    }

    Guid IMultiTenantEntity.BrandId
    {
        get => BrandId.Value;
        set => BrandId = new BrandId(value);
    }
}
