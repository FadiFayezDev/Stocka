using BuildingBlocks.Application.Common.Interfaces;
using Catalog.Application.Bindings.CategoryCases;
using Catalog.Application.DTOs.Category;
using Catalog.Application.Repositories.Queries.Category;
using Catalog.Contracts.DTOs.Category;
using Catalog.Contracts.UseCases.CategoryCases;
using MediatR;

namespace Catalog.Application.UseCases.Categories
{
    /// <summary>
    /// Lists categories of the current Organization (or an explicitly requested one).
    /// </summary>
    public class ListCategoriesQueryHandler : IRequestHandler<ListCategoriesQueryMediatR, IEnumerable<CategoryDto>>
    {
        private readonly ICategoryQueryRepository _categoryQueryRepository;
        private readonly ICurrentUserContext _currentUserContext;

        public ListCategoriesQueryHandler(ICategoryQueryRepository categoryQueryRepository, ICurrentUserContext currentUserContext)
        {
            _categoryQueryRepository = categoryQueryRepository;
            _currentUserContext = currentUserContext;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(ListCategoriesQueryMediatR request, CancellationToken cancellationToken)
        {
            var organizationId = request.Contract.OrganizationId == Guid.Empty
                ? _currentUserContext.ActiveOrganizationId
                : request.Contract.OrganizationId;

            return await _categoryQueryRepository.GetAllAsync(organizationId);
        }
    }
}
