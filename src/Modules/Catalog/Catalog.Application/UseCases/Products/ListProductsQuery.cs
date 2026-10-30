using BuildingBlocks.Application.Common.Interfaces;
using Catalog.Application.Bindings.ProductCases;
using Catalog.Application.DTOs.Product;
using Catalog.Application.Repositories.Queries.Product;
using Catalog.Contracts.DTOs.Product;
using Catalog.Contracts.UseCases.ProductCases;
using MediatR;

namespace Catalog.Application.UseCases.Products
{
    /// <summary>
    /// Lists products of the current Organization (or an explicitly requested one).
    /// </summary>
    public class ListProductsQueryHandler : IRequestHandler<ListProductsQueryMediatR, IEnumerable<ProductDto>>
    {
        private readonly IProductQueryRepository _productQueryRepository;
        private readonly ICurrentUserContext _currentContext;

        public ListProductsQueryHandler(IProductQueryRepository productQueryRepository, ICurrentUserContext currentContext)
        {
            _productQueryRepository = productQueryRepository;
            _currentContext = currentContext;
        }

        public async Task<IEnumerable<ProductDto>> Handle(ListProductsQueryMediatR request, CancellationToken cancellationToken)
        {
            var organizationId = request.Contract.OrganizationId ?? _currentContext.ActiveOrganizationId;
            var products = await _productQueryRepository.GetAllAsync(organizationId);
            return products;
        }
    }
}
