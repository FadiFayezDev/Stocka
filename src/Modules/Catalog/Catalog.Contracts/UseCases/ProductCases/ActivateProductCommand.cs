namespace Catalog.Contracts.UseCases.ProductCases;

/// <summary>
/// Represents a command to activate a product by its unique identifier.
/// </summary>
/// <param name="ProductId">The unique identifier of the product to activate.</param>
public record ActivateProductCommand(Guid ProductId);
