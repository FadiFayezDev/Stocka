using BuildingBlocks.Application.Abstractions;
using Catalog.Application.DTOs.Product;
using Catalog.Application.Repositories.Queries.Product;
using MediatR;

namespace Catalog.Application.UseCases.Products
{
    public class ListProductsCommand : IRequest<IEnumerable<ProductDto>>
    {
        // Optional explicit BrandId override; if empty uses current tenant
        public Guid? BrandId { get; set; }
    }

    public class ListProductsCommandHandler : IRequestHandler<ListProductsCommand, IEnumerable<ProductDto>>
    {
        private readonly IProductQueryRepository _productQueryRepository;
        private readonly ITenantContext _tenantContext;
        public ListProductsCommandHandler(IProductQueryRepository productQueryRepository, ITenantContext tenantContext)
        {
            _productQueryRepository = productQueryRepository;
            _tenantContext = tenantContext;
        }
        public async Task<IEnumerable<ProductDto>> Handle(ListProductsCommand request, CancellationToken cancellationToken)
        {
            var brandId = request.BrandId.HasValue && request.BrandId.Value != Guid.Empty
                ? BuildingBlocks.Application.Primitives.Identifiers.BrandId.FromGuid(request.BrandId.Value)
                : _tenantContext.BrandId;
            var products = await _productQueryRepository.GetAllAsync(brandId);
            return products;
        }
    }
}
