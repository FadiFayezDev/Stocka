namespace Catalog.Contracts.UseCases.ProductCases;

/// <summary>
/// Retrieve one product by ID. Cross-Organization IDs are indistinguishable from not-found.
/// </summary>
/// <param name="ProductId">The unique identifier of the product to retrieve.</param>
public record RetrieveProductQuery(Guid ProductId);
