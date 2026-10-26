using Catalog.Application.Common.Interfaces;
using Catalog.Application.DTOs.Product;
using Catalog.Application.Repositories.Commands;
using Catalog.Domain.Entities;
using Catalog.Domain.Primitives.Identifiers;
using MediatR;

namespace Catalog.Application.UseCases.Products
{
    public record ActivateProductCommand : ICommand<ProductDto>
    {
        public Guid ProductId { get; set; }
    }

    public class ActivateProductCommandHandler : IRequestHandler<ActivateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;

        public ActivateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> Handle(ActivateProductCommand request, CancellationToken cancellationToken)
        {
            if (request.ProductId == Guid.Empty)
                throw new ArgumentException("Product ID is required.", nameof(request.ProductId));

            var product = await _productRepository.GetByIdAsync(ProductId.FromGuid(request.ProductId))
                ?? throw new KeyNotFoundException($"Product with id '{request.ProductId}' was not found.");

            product.Activate();

            await _productRepository.UpdateAsync(product);

            return product.ToDto();
        }
    }
}