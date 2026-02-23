using Application.Bases;
using Application.Dtos.Products;
using Application.QueryRepositories;
using MediatR;

namespace Application.Queries.Product.GetProductsWithWarehousesQuntity
{
    public class GetProductsWithQuantitiesQuery : IRequest<Response<IEnumerable<ProductDto>>>
    {
        public Guid BrandId { get; set; }
        public GetProductsWithQuantitiesQuery(Guid brandId)
        {
            BrandId = brandId;
        }
    }

    public class GetProductsWithWarehouseQuntityQueryHandler : BaseHandler<IProductQueryRepository>, IRequestHandler<GetProductsWithQuantitiesQuery, Response<IEnumerable<ProductDto>>>
    {
        public GetProductsWithWarehouseQuntityQueryHandler(IProductQueryRepository repo) : base(repo)
        {
        }

        public async Task<Response<IEnumerable<ProductDto>>> Handle(
            GetProductsWithQuantitiesQuery request, CancellationToken cancellationToken)
        {
            var products = await _repo.GetProductsWithQuantities(request.BrandId);
            return new ResponseHandler().Success(products);
        }
    }
}