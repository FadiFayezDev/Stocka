using Application.Common.Interfaces;
using Application.Dtos.Products;
using Application.QueryRepositories;
using MediatR;

namespace Application.UseCases.ProductCases
{
    public class ListProductsCommand : IRequest<List<ProductDto>>
    {
    }

    public class ListProductsCommandHandler : IRequestHandler<ListProductsCommand, List<ProductDto>>
    {
        private readonly IProductQueryRepository _productQuery;
        private readonly ICurrentUserContext _userContext;

        public ListProductsCommandHandler(IProductQueryRepository branchQuery, ICurrentUserContext userContext)
        {
            _productQuery = branchQuery;
            _userContext = userContext;
        }

        public async Task<List<ProductDto>> Handle(ListProductsCommand request, CancellationToken cancellationToken)
        {
            var brandId = _userContext.ActiveBrandId;
            var products = await _productQuery.GetProductsWithQuantities(brandId);
            return products.ToList();
        }
    }
}