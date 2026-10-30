namespace Catalog.Contracts.UseCases.CategoryCases;

/// <summary>
/// Lists categories of the current Organization (or an explicitly requested one).
/// </summary>
public record ListCategoriesQuery
{
    /// <summary>Optional explicit organization override; empty uses the current Organization.</summary>
    public Guid OrganizationId { get; init; } = Guid.Empty;
}
