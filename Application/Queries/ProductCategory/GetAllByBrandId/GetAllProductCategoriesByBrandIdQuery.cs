using Application.Bases;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.ProductCategory.GetByBrandId
{
    public class GetAllProductCategoriesByBrandIdQuery : IRequest<Response<IEnumerable<ProductCategoryDto>>>
    {
        public Guid Id { get; set; }
        public GetAllProductCategoriesByBrandIdQuery(Guid brandId)
        {
            Id = brandId;
        }
    }

    public class GetAllProductCategoriesByBrandIdQueryHandler : IRequestHandler<GetAllProductCategoriesByBrandIdQuery, Response<IEnumerable<ProductCategoryDto>>>
    {
        private readonly IProductCategoryQueryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllProductCategoriesByBrandIdQueryHandler(IProductCategoryQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<ProductCategoryDto>>> Handle(GetAllProductCategoriesByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var productCategory = await _repository.GetAllByBrandIdAsync(request.Id);
            if (productCategory == null)
                return new Response<IEnumerable<ProductCategoryDto>>("ProductCategory not found");

            return new Response<IEnumerable<ProductCategoryDto>>(productCategory, "Success");
        }
    }
}