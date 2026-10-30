namespace Catalog.Contracts.UseCases.CategoryCases;

/// <summary>
/// Represents a command to activate a category by its unique identifier.
/// </summary>
/// <param name="CategoryId">The unique identifier of the category to activate.</param>
public record ActivateCategoryCommand(Guid CategoryId);
