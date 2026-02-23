using Domain.Bases;
using Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities.Products;

public partial class ProductCategory : IEntity<Guid>
{
    public Guid Id { get; set; }

    public Guid BrandId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    private readonly List<Product> _products = new();
    public virtual Brand Brand { get; set; } = null!;
    public virtual ICollection<Product> Products => _products.AsReadOnly();

    private ProductCategory() { }

    public ProductCategory(Guid brandId, string name)
    {
        Id = Guid.NewGuid();

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

    public void RemoveProduct(Guid productId)
    {
        var product = _products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
            throw new ArgumentException("Product not found in this category.");

        _products.Remove(product);
    }

    public Guid GetKey() => Id;

    public void SetKey(Guid key) => Id = key;
}
