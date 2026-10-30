using Catalog.Contracts.DTOs.Category;

namespace Catalog.Contracts.UseCases.CategoryCases;

/// <summary>
/// Category use case contract: the pure module boundary for every category operation.
/// Implemented by the Catalog Application layer (which wires each call into its MediatR
/// pipeline — validation and unit-of-work included) and consumed via DI.
/// </summary>
public interface ICategoryCases
{
    #region Commands
    /// <summary>Registers a new category for the current Organization.</summary>
    Task<CategoryDto?> RegisterCategoryAsync(RegisterCategoryCommand command, CancellationToken cancellationToken = default);

    /// <summary>Renames an existing category.</summary>
    Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryCommand command, CancellationToken cancellationToken = default);

    /// <summary>Activates an existing category.</summary>
    Task<CategoryDto> ActivateCategoryAsync(ActivateCategoryCommand command, CancellationToken cancellationToken = default);

    /// <summary>Deactivates an existing category (non-destructive; history is kept).</summary>
    Task<CategoryDto> DeactivateCategoryAsync(DeactivateCategoryCommand command, CancellationToken cancellationToken = default);
    #endregion

    #region Queries
    /// <summary>Retrieves one category by ID (cross-Organization IDs are indistinguishable from not-found).</summary>
    Task<CategoryDto> RetrieveCategoryAsync(RetrieveCategoryQuery query, CancellationToken cancellationToken = default);

    /// <summary>Retrieves one category by its exact name.</summary>
    Task<CategoryDto> RetrieveCategoryByNameAsync(RetrieveCategoryByNameQuery query, CancellationToken cancellationToken = default);

    /// <summary>Retrieves the category listing for the current (or explicitly requested) Organization.</summary>
    Task<IEnumerable<CategoryDto>> ListCategoriesAsync(ListCategoriesQuery query, CancellationToken cancellationToken = default);
    #endregion
}
