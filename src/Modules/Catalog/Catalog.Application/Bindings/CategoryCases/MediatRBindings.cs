using Catalog.Application.Common.Interfaces;
using Catalog.Contracts.DTOs.Category;
using Catalog.Contracts.UseCases.CategoryCases;
using MediatR;

namespace Catalog.Application.Bindings.CategoryCases;

public record RegisterCategoryCommandMediatR(RegisterCategoryCommand Contract) : ICommand<CategoryDto?>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record UpdateCategoryCommandMediatR(UpdateCategoryCommand Contract) : ICommand<CategoryDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record ActivateCategoryCommandMediatR(ActivateCategoryCommand Contract) : ICommand<CategoryDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record DeactivateCategoryCommandMediatR(DeactivateCategoryCommand Contract) : ICommand<CategoryDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record RetrieveCategoryQueryMediatR(RetrieveCategoryQuery Contract) : IRequest<CategoryDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record RetrieveCategoryByNameQueryMediatR(RetrieveCategoryByNameQuery Contract) : IRequest<CategoryDto>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
public record ListCategoriesQueryMediatR(ListCategoriesQuery Contract) : IRequest<IEnumerable<CategoryDto>>, IContractRequest
{
    object IContractRequest.Contract => Contract;
}
