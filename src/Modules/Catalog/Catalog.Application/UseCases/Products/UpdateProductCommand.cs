using Catalog.Application.Common.Interfaces;
using Catalog.Application.DTOs.Product;
using Catalog.Application.Repositories.Commands;
using Catalog.Application.Repositories.Queries.Category;
using Catalog.Domain.Entities;
using Catalog.Domain.Primitives;
using Catalog.Domain.Primitives.Identifiers;
using MediatR;

namespace Catalog.Application.UseCases.Products
{
    public record UpdateProductCommand : ICommand<ProductDto>
    {
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal? SellingPrice { get; set; }
        public string? Barcode { get; set; }
        public string? ImagePath { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryQueryRepository _categoryQueryRepository;

        public UpdateProductCommandHandler(
            IProductRepository productRepository,
            ICategoryQueryRepository categoryQueryRepository)
        {
            _productRepository = productRepository;
            _categoryQueryRepository = categoryQueryRepository;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if (request.ProductId == Guid.Empty)
                throw new ArgumentException("Product ID is required.", nameof(request.ProductId));

            var product = await _productRepository.GetByIdAsync(ProductId.FromGuid(request.ProductId))
                ?? throw new KeyNotFoundException($"Product with id '{request.ProductId}' was not found.");

            bool changeRequested = false;

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                product.Rename(request.Name);
                changeRequested = true;
            }

            if (request.SellingPrice.HasValue)
            {
                product.ChangeSellingPrice(request.SellingPrice.Value);
                changeRequested = true;
            }

            if (request.CategoryId.HasValue)
            {
                if (request.CategoryId.Value == Guid.Empty)
                    throw new ArgumentException("Category ID must be a valid value.", nameof(request.CategoryId));

                var categoryExists = await _categoryQueryRepository.GetByIdAsync(request.CategoryId.Value);
                if (categoryExists is null)
                    throw new KeyNotFoundException($"Category with id '{request.CategoryId}' was not found.");

                product.ChangeCategory(CategoryId.FromGuid(request.CategoryId.Value));
                changeRequested = true;
            }

            if (request.Barcode is not null)
            {
                product.ChangeBarcode(request.Barcode);
                changeRequested = true;
            }

            if (request.ImagePath is not null)
            {
                product.ChangeImage(request.ImagePath);
                changeRequested = true;
            }

            if (!changeRequested)
                throw new ArgumentException("At least one property must be provided to update the product.", nameof(request));

            await _productRepository.UpdateAsync(product);

            return product.ToDto();
        }
    }
}