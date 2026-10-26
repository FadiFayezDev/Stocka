using BuildingBlocks.Application.Primitives.Identifiers;
using BuildingBlocks.Domain.Bases;
using Catalog.Domain.Primitives;
using System.Diagnostics.CodeAnalysis;

namespace Catalog.Domain.Entities;

public partial class Category : AggregateRoot<CategoryId>/*, IMultiTenantEntity*/
{
    public BrandId BrandId { get; private set; }

    public string Name { get; private set; } = null!;

    public bool IsActive { get; private set; }

    private Category() { }

    [SetsRequiredMembers]
    public Category(BrandId brandId, string name)
    {
        Id = CategoryId.New();

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty.", nameof(name));

        BrandId = brandId;
        Name = name.Trim();
        IsActive = true;
    }

    public static Category Create(BrandId brandId, string name)
    {
        return new Category(brandId, name);
    }

    public void UpdateActive(bool value) => IsActive = value;

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Category name cannot be empty.", nameof(newName));

        Name = newName.Trim();
    }

    //Guid IMultiTenantEntity.BrandId
    //{
    //    get => BrandId.Value;
    //    set => BrandId = new BrandId(value);
    //}
}