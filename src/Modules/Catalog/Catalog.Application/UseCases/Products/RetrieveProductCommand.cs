using Catalog.Application.DTOs.Product;
using Catalog.Application.Repositories.Queries.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.UseCases.Products
{
    public class RetrieveProductCommand : IRequest<ProductDto>
    {
        public Guid ProductId { get; set; }

        public RetrieveProductCommand(Guid productId)
        {
            ProductId = productId;
        }
    }

    public class RetrieveProductCommandHandler : IRequestHandler<RetrieveProductCommand, ProductDto>
    {
        private readonly IProductQueryRepository _productQuery;

        public RetrieveProductCommandHandler(IProductQueryRepository productQuery)
        {
            _productQuery = productQuery;
        }

        public async Task<ProductDto> Handle(RetrieveProductCommand request, CancellationToken cancellationToken)
        {
            if (request.ProductId == Guid.Empty)
                throw new ArgumentException("ProductId cannot be empty.", nameof(request.ProductId));

            var product = await _productQuery.GetByIdAsync(request.ProductId);
            return product ?? throw new KeyNotFoundException($"Product '{request.ProductId}' is not found.");
        }
    }
}
