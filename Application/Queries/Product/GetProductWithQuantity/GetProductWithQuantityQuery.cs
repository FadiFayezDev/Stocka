using Application.Bases;
using Application.Dtos.Products;
using Application.QueryRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Queries.Product.GetProductWithQuantity
{
    public class GetProductWithQuantityQuery : IRequest<ProductDto>
    {
        public Guid ProductId { get; set; }
        public GetProductWithQuantityQuery(Guid productId)
        {
            ProductId = productId;
        }
    }

    public class GetProductWithQuantityQueryHandler : BaseHandler<IProductQueryRepository>, IRequestHandler<GetProductWithQuantityQuery, ProductDto>
    {
        public GetProductWithQuantityQueryHandler(IProductQueryRepository repo) : base(repo)
        {
        }

        public async Task<ProductDto> Handle(GetProductWithQuantityQuery request, CancellationToken cancellationToken)
        {
            var product = await _repo.GetProductsWithQuantityAsync(request.ProductId);
            if (product == null)
                throw new KeyNotFoundException("Product not found");
            return product;
        }
    }
}