using Application.Bases;
using Application.Dtos.Orders;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Sale.GetByBrandId
{
    public class GetAllOrdersByBrandIdQuery : IRequest<Response<IEnumerable<OrderDto>>>
    {
        public Guid BrandId { get; set; }

        public GetAllOrdersByBrandIdQuery(Guid brandId)
        {
            BrandId = brandId;
        }
    }

    public class GetAllOrdersByBrandIdQueryHandler : IRequestHandler<GetAllOrdersByBrandIdQuery, Response<IEnumerable<OrderDto>>>
    {
        private readonly IOrderQueryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllOrdersByBrandIdQueryHandler(IOrderQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<OrderDto>>> Handle(GetAllOrdersByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _repository.GetAllByBrandIdAsync(request.BrandId);
            if (orders == null)
                return new Response<IEnumerable<OrderDto>>("Orders not found");

            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return new Response<IEnumerable<OrderDto>>(orderDtos, "Success");
        }
    }
}
