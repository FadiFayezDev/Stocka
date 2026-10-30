using Catalog.Application.Bindings.CategoryCases;
using Catalog.Contracts.DTOs.Category;
using Catalog.Contracts.UseCases.CategoryCases;
using MediatR;

namespace Catalog.Application.Adapters.CategoryCases;

/// <summary>
/// Implements the ICategoryCases contract by dispatching each pure contract record through the
/// application's MediatR pipeline (validation behavior, unit-of-work behavior, handler).
/// Registered in DI as the implementation of the module boundary contract.
/// </summary>
public class CategoryMediatorCases : ICategoryCases
{
    private readonly IMediator _mediator;

    public CategoryMediatorCases(IMediator mediator) => _mediator = mediator;

    public Task<CategoryDto?> RegisterCategoryAsync(RegisterCategoryCommand command, CancellationToken cancellationToken = default)
        => _mediator.Send(new RegisterCategoryCommandMediatR(command), cancellationToken);

    public Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryCommand command, CancellationToken cancellationToken = default)
        => _mediator.Send(new UpdateCategoryCommandMediatR(command), cancellationToken);

    public Task<CategoryDto> ActivateCategoryAsync(ActivateCategoryCommand command, CancellationToken cancellationToken = default)
        => _mediator.Send(new ActivateCategoryCommandMediatR(command), cancellationToken);

    public Task<CategoryDto> DeactivateCategoryAsync(DeactivateCategoryCommand command, CancellationToken cancellationToken = default)
        => _mediator.Send(new DeactivateCategoryCommandMediatR(command), cancellationToken);

    public Task<CategoryDto> RetrieveCategoryAsync(RetrieveCategoryQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new RetrieveCategoryQueryMediatR(query), cancellationToken);

    public Task<CategoryDto> RetrieveCategoryByNameAsync(RetrieveCategoryByNameQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new RetrieveCategoryByNameQueryMediatR(query), cancellationToken);

    public Task<IEnumerable<CategoryDto>> ListCategoriesAsync(ListCategoriesQuery query, CancellationToken cancellationToken = default)
        => _mediator.Send(new ListCategoriesQueryMediatR(query), cancellationToken);
}
