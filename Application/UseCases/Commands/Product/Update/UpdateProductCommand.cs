using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Product.Update
{
    public class UpdateProductCommand : IRequest<Response<ProductDto>>
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Barcode { get; set; }
    }

    public class UpdateProductCommandHandler : BaseHandler<IProductCommandRepository>, IRequestHandler<UpdateProductCommand, Response<ProductDto>>
    {
        public UpdateProductCommandHandler(IProductCommandRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, productRepository, unitOfWork)
        {
        }

        public async Task<Response<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await _repo.GetByIdAsync(request.Id);
            if (existingProduct == null)
                return new Response<ProductDto>("Product not found");

            _mapper.Map(request, existingProduct);

            return await ExecuteUpdateAsync<Domain.Entities.Products.Product, ProductDto>(
                existingProduct,
                async (p) => await _repo.UpdateAsync(p),
                cancellationToken);
        }
    }
}
