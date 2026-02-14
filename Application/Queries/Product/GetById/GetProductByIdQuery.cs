using Application.Bases;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.Product.GetById
{
    public class GetProductByIdQuery : IRequest<Response<ProductDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetProductByIdQueryHandler : BaseHandler<IProductCommandRepository>, IRequestHandler<GetProductByIdQuery, Response<ProductDto>>
    {
        public GetProductByIdQueryHandler(IProductCommandRepository productRepository, IMapper mapper) : base(mapper, productRepository)
        {
        }

        public async Task<Response<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _repo.GetByIdAsync(request.Id);
            if (product == null)
                return new Response<ProductDto>("Product not found");

            var productDto = _mapper.Map<ProductDto>(product);
            return new Response<ProductDto>(productDto, "Success");
        }
    }
}
