using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.Product.GetAll
{
    public class GetAllProductsQuery : IRequest<Response<IEnumerable<ProductDto>>>
    {
    }

    public class GetAllProductsQueryHandler : BaseHandler<IProductCommandRepository>, IRequestHandler<GetAllProductsQuery, Response<IEnumerable<ProductDto>>>
    {
        private readonly ICurrentUserContext _currentUser;

        public GetAllProductsQueryHandler(IProductCommandRepository productRepository, ICurrentUserContext currentUser, IMapper mapper) : base(mapper, productRepository)
        {
            this._currentUser = currentUser;
        }

        public async Task<Response<IEnumerable<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;
            var products = await _repo.GetAllTableAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return new Response<IEnumerable<ProductDto>>(productDtos, "Success");
        }
    }
}
