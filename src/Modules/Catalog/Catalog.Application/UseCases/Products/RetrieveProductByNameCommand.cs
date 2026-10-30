using Catalog.Application.Bindings.ProductCases;
using Catalog.Application.DTOs.Product;
using Catalog.Application.Repositories.Queries.Product;
using Catalog.Contracts.DTOs.Product;
using Catalog.Contracts.UseCases.ProductCases;
using MediatR;

namespace Catalog.Application.UseCases.Products
{
    /// <summary>
    /// Retrieves one product by its exact name; missing names raise KeyNotFoundException.
    /// </summary>
    public class RetrieveProductByNameQueryHandler : IRequestHandler<RetrieveProductByNameQueryMediatR, ProductDto>
    {
        private readonly IProductQueryRepository _productQueryRepository;

        public RetrieveProductByNameQueryHandler(IProductQueryRepository productQueryRepository)
        {
            _productQueryRepository = productQueryRepository;
        }

        public async Task<ProductDto> Handle(RetrieveProductByNameQueryMediatR request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Contract.Name))
                throw new ArgumentException("Product name is required.", nameof(request.Contract.Name));

            return await _productQueryRepository.GetByNameAsync(request.Contract.Name.Trim())
                ?? throw new KeyNotFoundException($"Product with name '{request.Contract.Name}' was not found.");
        }
    }
}
