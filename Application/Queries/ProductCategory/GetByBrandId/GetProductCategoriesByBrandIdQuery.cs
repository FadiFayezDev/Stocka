using Application.Bases;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.ProductCategory.GetByBrandId
{
    public class GetProductCategoriesByBrandIdQuery : IRequest<Response<IEnumerable<ProductCategoryIncludedBrandDto>>>
    {
        public Guid Id { get; set; }
        public GetProductCategoriesByBrandIdQuery(Guid brandId)
        {
            Id = brandId;
        }
    }

    public class GetProductCategoriesByBrandIdQueryHandler : IRequestHandler<GetProductCategoriesByBrandIdQuery, Response<IEnumerable<ProductCategoryIncludedBrandDto>>>
    {
        private readonly IProductCategoryQueryRepository _repository;
        private readonly IMapper _mapper;

        public GetProductCategoriesByBrandIdQueryHandler(IProductCategoryQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<ProductCategoryIncludedBrandDto>>> Handle(GetProductCategoriesByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var productCategory = await _repository.GetAllProductCategoriesWithBrandsAsync(request.Id);
            if (productCategory == null)
                return new Response<IEnumerable<ProductCategoryIncludedBrandDto>>(false, "ProductCategory not found");

            var productCategoryDto = _mapper.Map<IEnumerable<ProductCategoryIncludedBrandDto>>(productCategory);
            return new Response<IEnumerable<ProductCategoryIncludedBrandDto>>(productCategoryDto, "Success");
        }
    }
}
