namespace Catalog.Contracts.UseCases.ProductCases;

/// <summary>
/// Represents a command to deactivate a product by its unique identifier (non-destructive).
/// </summary>
/// <param name="ProductId">The unique identifier of the product to deactivate.</param>
public record DeactivateProductCommand(Guid ProductId);
