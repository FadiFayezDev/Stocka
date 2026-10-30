namespace Catalog.Contracts.UseCases.CategoryCases;

/// <summary>
/// Represents a rename of an existing category.
/// </summary>
/// <param name="CategoryId">The unique identifier of the category to update.</param>
/// <param name="Name">New name; must be provided.</param>
public record UpdateCategoryCommand
{
    /// <summary>The unique identifier of the category to update.</summary>
    public Guid CategoryId { get; init; }

    /// <summary>New name; required.</summary>
    public string? Name { get; init; }
}
