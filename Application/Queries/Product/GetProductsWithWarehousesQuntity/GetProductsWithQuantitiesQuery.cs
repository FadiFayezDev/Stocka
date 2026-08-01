using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using Application.QueryRepositories;
using MediatR;

namespace Application.Queries.Product.GetProductsWithWarehousesQuntity
{
    public class GetProductsWithQuantitiesQuery : IRequest<Response<IEnumerable<ProductDto>>>
    {
    }

    public class GetProductsWithWarehouseQuntityQueryHandler : BaseHandler<IProductQueryRepository>, IRequestHandler<GetProductsWithQuantitiesQuery, Response<IEnumerable<ProductDto>>>
    {
        private readonly ICurrentUserContext _currentUserContext;
        public GetProductsWithWarehouseQuntityQueryHandler(IProductQueryRepository repo, ICurrentUserContext currentUserContext) : base(repo)
        {
            _currentUserContext = currentUserContext;
        }

        public async Task<Response<IEnumerable<ProductDto>>> Handle(
            GetProductsWithQuantitiesQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUserContext.ActiveBrandId;

            var products = await _repo.GetProductsWithQuantities(brandId);
            return new ResponseHandler().Success(products);
        }
    }
}