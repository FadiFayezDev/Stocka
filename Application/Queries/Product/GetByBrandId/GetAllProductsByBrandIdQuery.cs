using Application.Bases;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Product.GetByBrandId
{
    public class GetAllProductsByBrandIdQuery : IRequest<Response<IEnumerable<ProductDto>>>
    {
        public Guid BrandId { get; set; }

        public GetAllProductsByBrandIdQuery(Guid brandId)
        {
            BrandId = brandId;
        }
    }

    public class GetAllProductsByBrandIdQueryHandler : IRequestHandler<GetAllProductsByBrandIdQuery, Response<IEnumerable<ProductDto>>>
    {
        private readonly IProductQueryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllProductsByBrandIdQueryHandler(IProductQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<ProductDto>>> Handle(GetAllProductsByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var products = await _repository.GetAllByBrandIdAsync(request.BrandId);
            if (products == null)
                return new Response<IEnumerable<ProductDto>>("Products not found");

            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return new Response<IEnumerable<ProductDto>>(productDtos, "Success");
        }
    }
}
