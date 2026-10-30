namespace Catalog.Contracts.UseCases.CategoryCases;

/// <summary>
/// Retrieve one category by its exact (trimmed) name within the current Organization.
/// </summary>
/// <param name="Name">The exact category name to look up.</param>
public record RetrieveCategoryByNameQuery(string Name);
