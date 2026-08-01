using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.ProductCategory.GetAll
{
    public class GetAllProductCategoriesQuery : IRequest<Response<IEnumerable<ProductCategoryDto>>>
    {
    }

    public class GetAllProductCategoriesQueryHandler : BaseHandler<IProductCategoryQueryRepository>, IRequestHandler<GetAllProductCategoriesQuery, Response<IEnumerable<ProductCategoryDto>>>
    {
        private readonly ICurrentUserContext _currentUser;
        public GetAllProductCategoriesQueryHandler(IProductCategoryQueryRepository productCategoryRepository, IMapper mapper, ICurrentUserContext currentUser) : base(mapper, productCategoryRepository)
        {
            _currentUser = currentUser;
        }

        public async Task<Response<IEnumerable<ProductCategoryDto>>> Handle(GetAllProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;
            
            var productCategories = await _repo.GetAllByBrandIdAsync(brandId);
            return new Response<IEnumerable<ProductCategoryDto>>(productCategories, "Success");
        }
    }
}
