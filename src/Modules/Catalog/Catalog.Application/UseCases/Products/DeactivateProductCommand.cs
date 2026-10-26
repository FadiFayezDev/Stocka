using Catalog.Application.Common.Interfaces;
using Catalog.Application.DTOs.Product;
using Catalog.Application.Repositories.Commands;
using Catalog.Domain.Entities;
using Catalog.Domain.Primitives.Identifiers;
using MediatR;

namespace Catalog.Application.UseCases.Products
{
    public record DeactivateProductCommand : ICommand<ProductDto>
    {
        public Guid ProductId { get; set; }
    }

    public class DeactivateProductCommandHandler : IRequestHandler<DeactivateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;

        public DeactivateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> Handle(DeactivateProductCommand request, CancellationToken cancellationToken)
        {
            if (request.ProductId == Guid.Empty)
                throw new ArgumentException("Product ID is required.", nameof(request.ProductId));

            var product = await _productRepository.GetByIdAsync(ProductId.FromGuid(request.ProductId))
                ?? throw new KeyNotFoundException($"Product with id '{request.ProductId}' was not found.");

            product.Deactivate();

            await _productRepository.UpdateAsync(product);

            return product.ToDto();
        }
    }
}