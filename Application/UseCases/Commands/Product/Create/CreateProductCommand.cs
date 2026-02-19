using Application.Bases;
using Application.Common.Interfaces;
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
        public CreateProductCommandHandler(IProductCommandRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, productRepository, unitOfWork)
        {
        }

        public async Task<Response<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Domain.Entities.Products.Product>(request);

            return await ExecuteCreateAsync<Domain.Entities.Products.Product, ProductDto>(
                product,
                async (p) => await _repo.CreateAsync(p),
                cancellationToken);
        }
    }
}
