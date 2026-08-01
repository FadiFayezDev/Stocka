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

    private readonly List<Product> _products = new();
    public virtual Brand Brand { get; private set; } = null!;
    public virtual ICollection<Product> Products => _products.AsReadOnly();

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

    public void AddProduct(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        if (product.CategoryId != Id)
            throw new ArgumentException("Product does not belong to this category.");

        if (_products.Any(p => p.Id == product.Id))
            throw new InvalidOperationException("Product already exists in this category.");

        _products.Add(product);
    }

    public void RemoveProduct(ProductId productId)
    {
        var product = _products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
            throw new ArgumentException("Product not found in this category.");

        _products.Remove(product);
    }

    Guid IMultiTenantEntity.BrandId
    {
        get => BrandId.Value;
        set => BrandId = new BrandId(value);
    }
}
