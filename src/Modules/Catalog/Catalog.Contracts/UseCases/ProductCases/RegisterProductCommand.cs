namespace Catalog.Contracts.UseCases.ProductCases;

/// <summary>
/// Registers a new product for the current Organization.
/// </summary>
/// <remarks>
/// Rules are enforced inside the Application pipeline (handler + behaviors);
/// the domain factory additionally enforces name/price invariants.
/// </remarks>
public record RegisterProductCommand
{
    /// <summary>Display name (unique per the current Organization).</summary>
    public string Name { get; init; } = default!;

    /// <summary>Optional product barcode (database NVARCHAR(32)).</summary>
    public string Barcode { get; init; } = default!;

    /// <summary>Selling price; cannot be negative.</summary>
    public decimal SellingPrice { get; init; }

    /// <summary>Owning category identifier.</summary>
    public Guid CategoryId { get; init; }
}
