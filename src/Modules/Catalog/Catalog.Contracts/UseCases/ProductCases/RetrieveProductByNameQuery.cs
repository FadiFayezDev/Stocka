namespace Catalog.Contracts.UseCases.ProductCases;

/// <summary>
/// Retrieve one product by its exact (trimmed) name within the current Organization.
/// </summary>
/// <param name="Name">The exact product name to look up.</param>
public record RetrieveProductByNameQuery(string Name);
