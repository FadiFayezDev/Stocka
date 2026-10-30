namespace Catalog.Contracts.UseCases.CategoryCases;

/// <summary>
/// Registers a new category for the current Organization.
/// </summary>
/// <remarks>
/// Rules are enforced inside the Application pipeline (handler + behaviors);
/// the domain factory additionally enforces name invariants.
/// </remarks>
public record RegisterCategoryCommand
{
    /// <summary>Display name.</summary>
    public string Name { get; init; } = default!;
}
