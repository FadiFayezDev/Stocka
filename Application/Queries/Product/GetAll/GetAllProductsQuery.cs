using Application.Bases;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.Product.GetAll
{
    public class GetAllProductsQuery : IRequest<Response<IEnumerable<ProductDto>>>
    {
    }

    public class GetAllProductsQueryHandler : BaseHandler<IProductCommandRepository>, IRequestHandler<GetAllProductsQuery, Response<IEnumerable<ProductDto>>>
    {
        public GetAllProductsQueryHandler(IProductCommandRepository productRepository, IMapper mapper) : base(mapper, productRepository)
        {
        }

        public async Task<Response<IEnumerable<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _repo.GetAllTableAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return new Response<IEnumerable<ProductDto>>(productDtos, "Success");
        }
    }
}
