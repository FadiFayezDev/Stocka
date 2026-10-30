namespace Catalog.Contracts.UseCases.ProductCases;

/// <summary>
/// Represents a partial update of an existing product: only provided (non-null)
/// fields change; unspecified fields keep their current values.
/// </summary>
/// <remarks>
/// At least one property must be provided, otherwise the request is rejected.
/// </remarks>
public record UpdateProductCommand
{
    /// <summary>The unique identifier of the product to update.</summary>
    public Guid ProductId { get; init; }

    /// <summary>New name; null keeps the current value.</summary>
    public string? Name { get; init; }

    /// <summary>New category identifier; null keeps the current value.</summary>
    public Guid? CategoryId { get; init; }

    /// <summary>New selling price; null keeps the current value.</summary>
    public decimal? SellingPrice { get; init; }

    /// <summary>New barcode; null keeps the current value.</summary>
    public string? Barcode { get; init; }

    /// <summary>New image path; null keeps the current value.</summary>
    public string? ImagePath { get; init; }
}
