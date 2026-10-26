using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Primitives.Identifiers;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.DTOs.Product;
using Catalog.Application.Repositories.Commands;
using Catalog.Application.Repositories.Queries.Product;
using Catalog.Domain.Entities;
using Catalog.Domain.Primitives;
using MediatR;

namespace Catalog.Application.UseCases.Products
{
    public record RegisterProductCommand : ICommand<ProductDto?>
    {
        public string Name { get; set; } = default!;
        public string Barcode { get; set; } = default!;
        public decimal SellingPrice { get; set; }
        public Guid CategoryId { get; set; }
        public Stream? Image { get; set; }
    }

    public class RegisterProductCommandHandler : IRequestHandler<RegisterProductCommand, ProductDto?>
    {
        private readonly IProductRepository _productRepository;
        private readonly ITenantContext _tenantContext;

        public RegisterProductCommandHandler(IProductRepository productRepository, ITenantContext tenantContext)
        {
            _productRepository = productRepository;
            _tenantContext = tenantContext;
        }

        public async Task<ProductDto?> Handle(RegisterProductCommand request, CancellationToken cancellationToken)
        {
            // Business validations
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Name cannot be empty.", nameof(request.Name));

            if (request.CategoryId == Guid.Empty)
                throw new ArgumentException("Category ID cannot be empty.", nameof(request.CategoryId));

            if (request.SellingPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(request.SellingPrice), "Selling price cannot be negative.");

            // Ensure basic business rules before creating entity
            var product = Product.Create(
                _tenantContext.BrandId,
                CategoryId.FromGuid(request.CategoryId),
                request.Name.Trim(),
                request.SellingPrice,
                string.IsNullOrWhiteSpace(request.Barcode) ? null : request.Barcode.Trim());

            await _productRepository.AddAsync(product);

            return product.ToDto();
        }
    }
}