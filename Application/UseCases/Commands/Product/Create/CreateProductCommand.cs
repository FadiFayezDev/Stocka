using Application.Bases;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Product.Create
{
    public class CreateProductCommand : IRequest<Response<ProductDto>>
    {
        public Guid BrandId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Barcode { get; set; }
    }

    public class CreateProductCommandHandler : BaseHandler<IProductCommandRepository>, IRequestHandler<CreateProductCommand, Response<ProductDto>>
    {
        public CreateProductCommandHandler(IProductCommandRepository productRepository, IMapper mapper) : base(mapper, productRepository)
        {
        }

        public async Task<Response<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Domain.Entities.Products.Product>(request);

            await _repo.CreateAsync(product);

            return new Response<ProductDto>();
        }
    }
}
