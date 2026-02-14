using Application.Bases;
using Application.Dtos.Products;
using AutoMapper;
using MediatR;
using Domain.Repositories.Commands;

namespace Application.Features.Queries.ProductCategory.GetAll
{
    public class GetAllProductCategoriesQuery : IRequest<Response<IEnumerable<ProductCategoryDto>>>
    {
    }

    public class GetAllProductCategoriesQueryHandler : BaseHandler<IProductCategoryCommandRepository>, IRequestHandler<GetAllProductCategoriesQuery, Response<IEnumerable<ProductCategoryDto>>>
    {
        public GetAllProductCategoriesQueryHandler(IProductCategoryCommandRepository productCategoryRepository, IMapper mapper) : base(mapper, productCategoryRepository)
        {
        }

        public async Task<Response<IEnumerable<ProductCategoryDto>>> Handle(GetAllProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            var productCategories = await _repo.GetAllTableAsync();
            var productCategoryDtos = _mapper.Map<IEnumerable<ProductCategoryDto>>(productCategories);
            return new Response<IEnumerable<ProductCategoryDto>>(productCategoryDtos, "Success");
        }
    }
}
