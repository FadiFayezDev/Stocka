using Application.Common.Exceptions;
using Application.Dtos.Products;
using Application.QueryRepositories;
using MediatR;

namespace Application.UseCases.ProductCases
{
    public class RetrieveProductCommand : IRequest<ProductDto>
    {
        public Guid ProductId { get; set; }

        public RetrieveProductCommand(Guid branchId)
        {
            ProductId = branchId;
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
            var product = await _productQuery.GetProductWithQuantityAsync(request.ProductId);
            if (product == null)
                throw new BusinessException("Product is not found.");
            return product;
        }
    }
}