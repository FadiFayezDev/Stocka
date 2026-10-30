namespace Catalog.Contracts.UseCases.CategoryCases;

/// <summary>
/// Retrieve one category by ID. Cross-Organization IDs are indistinguishable from not-found.
/// </summary>
/// <param name="CategoryId">The unique identifier of the category to retrieve.</param>
public record RetrieveCategoryQuery(Guid CategoryId);
