using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Orders;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Sale.GetByBrandId
{
    /// <summary>
    /// The brand ID is injected automatically.
    /// </summary>
    public class GetAllOrdersByBrandIdQuery : IRequest<Response<IEnumerable<OrderDto>>>
    {

    }

    public class GetAllOrdersByBrandIdQueryHandler : IRequestHandler<GetAllOrdersByBrandIdQuery, Response<IEnumerable<OrderDto>>>
    {
        private readonly IOrderQueryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUser;

        public GetAllOrdersByBrandIdQueryHandler(IOrderQueryRepository repository, IMapper mapper, ICurrentUserContext currentUser)
        {
            _repository = repository;
            _mapper = mapper;
            _currentUser = currentUser;

        }

        public async Task<Response<IEnumerable<OrderDto>>> Handle(GetAllOrdersByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var orders = await _repository.GetAllByBrandIdAsync(brandId);
            if (orders == null)
                return new Response<IEnumerable<OrderDto>>("Orders not found");

            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return new Response<IEnumerable<OrderDto>>(orderDtos, "Success");
        }
    }
}