using Catalog.Application.DTOs.Product;
using Catalog.Application.Repositories.Queries.Product;
using MediatR;

namespace Catalog.Application.UseCases.Products
{
    public record RetrieveProductByNameCommand : IRequest<ProductDto>
    {
        public string Name { get; set; } = default!;
    }

    public class RetrieveProductByNameCommandHandler : IRequestHandler<RetrieveProductByNameCommand, ProductDto>
    {
        private readonly IProductQueryRepository _productQueryRepository;

        public RetrieveProductByNameCommandHandler(IProductQueryRepository productQueryRepository)
        {
            _productQueryRepository = productQueryRepository;
        }

        public async Task<ProductDto> Handle(RetrieveProductByNameCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Product name is required.", nameof(request.Name));

            return await _productQueryRepository.GetByNameAsync(request.Name.Trim())
                ?? throw new KeyNotFoundException($"Product with name '{request.Name}' was not found.");
        }
    }
}