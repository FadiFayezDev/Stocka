using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Queries.Product.GetProductsWithWarehousesQuntity
{
    public class GetProductsWithQuantitiesQuery : IRequest<Response<IEnumerable<ProductsWithWarehouseQuntityDto>>>
    {
        public Guid BrandId { get; set; }
        public GetProductsWithQuantitiesQuery(Guid brandId)
        {
            BrandId = brandId;
        }
    }

    public class GetProductsWithWarehouseQuntityQueryHandler : BaseHandler<IProductQueryRepository>, IRequestHandler<GetProductsWithQuantitiesQuery, Response<IEnumerable<ProductsWithWarehouseQuntityDto>>>
    {
        public GetProductsWithWarehouseQuntityQueryHandler(IProductQueryRepository repo) : base(repo)
        {
        }

        public async Task<Response<IEnumerable<ProductsWithWarehouseQuntityDto>>> Handle(
            GetProductsWithQuantitiesQuery request, CancellationToken cancellationToken)
        {
            var products = await _repo.GetProductsWithQuantities(request.BrandId);
            return new ResponseHandler().Success(products);
        }
    }
}