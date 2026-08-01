using Application.Common.Interfaces;
using Application.Dtos.Products;
using Application.QueryRepositories;
using MediatR;

namespace Application.UseCases.Category
{
    public class ListCategoriesCommand : IRequest<List<ProductCategoryDto>>
    {
    }

    public class ListCategoriesCommandHandler : IRequestHandler<ListCategoriesCommand, List<ProductCategoryDto>>
    {
        private readonly IProductCategoryQueryRepository _categoryQuery;
        private readonly ICurrentUserContext _userContext;

        public ListCategoriesCommandHandler(IProductCategoryQueryRepository categoryQuery, ICurrentUserContext userContext)
        {
            _categoryQuery = categoryQuery;
            _userContext = userContext;
        }

        public async Task<List<ProductCategoryDto>> Handle(ListCategoriesCommand request, CancellationToken cancellationToken)
        {
            var brandId = _userContext.ActiveBrandId;
            var categories = await _categoryQuery.GetAllByBrandIdAsync(brandId);
            return categories.ToList();
        }
    }
}
