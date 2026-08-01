using Application.Bases;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.ProductCategory.GetByIdWithBrand
{
    public class GetProductCategoryByIdWithBrandQuery : IRequest<Response<ProductCategoryIncludedBrandDto>>
    {
        public Guid Id { get; set; }
        public GetProductCategoryByIdWithBrandQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetProductCategoryByIdWithBrandQueryHandler : IRequestHandler<GetProductCategoryByIdWithBrandQuery, Response<ProductCategoryIncludedBrandDto>>
    {
        private readonly IProductCategoryQueryRepository _repository;
        private readonly IMapper _mapper;

        public GetProductCategoryByIdWithBrandQueryHandler(IProductCategoryQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<ProductCategoryIncludedBrandDto>> Handle(GetProductCategoryByIdWithBrandQuery request, CancellationToken cancellationToken)
        {
            var productCategory = await _repository.GetProductCategoryByIdWithBrandsAsync(request.Id);
            if (productCategory == null)
                return new Response<ProductCategoryIncludedBrandDto>("ProductCategory not found");

            var productCategoryDto = _mapper.Map<ProductCategoryIncludedBrandDto>(productCategory);
            return new Response<ProductCategoryIncludedBrandDto>(productCategoryDto, "Success");
        }
    }
}
