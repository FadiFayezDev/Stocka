namespace Catalog.Contracts.UseCases.CategoryCases;

/// <summary>
/// Represents a command to deactivate a category by its unique identifier (non-destructive).
/// </summary>
/// <param name="CategoryId">The unique identifier of the category to deactivate.</param>
public record DeactivateCategoryCommand(Guid CategoryId);
