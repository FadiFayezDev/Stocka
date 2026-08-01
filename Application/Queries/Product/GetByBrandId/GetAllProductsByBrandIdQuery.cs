using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Product.GetByBrandId
{
    public class GetAllProductsByBrandIdQuery : IRequest<Response<IEnumerable<ProductDto>>>
    {

    }

    public class GetAllProductsByBrandIdQueryHandler : IRequestHandler<GetAllProductsByBrandIdQuery, Response<IEnumerable<ProductDto>>>
    {
        private readonly IProductQueryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUser;

        public GetAllProductsByBrandIdQueryHandler(IProductQueryRepository repository, IMapper mapper, ICurrentUserContext currentUser)
        {
            _repository = repository;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<Response<IEnumerable<ProductDto>>> Handle(GetAllProductsByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var products = await _repository.GetAllByBrandIdAsync(brandId);
            if (products == null)
                return new Response<IEnumerable<ProductDto>>("Products not found");

            return new Response<IEnumerable<ProductDto>>(products, "Success");
        }
    }
}
