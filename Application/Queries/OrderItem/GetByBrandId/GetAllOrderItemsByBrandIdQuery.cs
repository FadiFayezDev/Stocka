using Application.Bases;
using Application.Dtos.Orders;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.OrderItem.GetByBrandId
{
    public class GetAllOrderItemsByBrandIdQuery : IRequest<Response<IEnumerable<OrderItemDto>>>
    {
        public Guid BrandId { get; set; }

        public GetAllOrderItemsByBrandIdQuery(Guid brandId)
        {
            BrandId = brandId;
        }
    }

    public class GetAllOrderItemsByBrandIdQueryHandler : IRequestHandler<GetAllOrderItemsByBrandIdQuery, Response<IEnumerable<OrderItemDto>>>
    {
        private readonly IOrderItemQueryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllOrderItemsByBrandIdQueryHandler(IOrderItemQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<OrderItemDto>>> Handle(GetAllOrderItemsByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var orderItems = await _repository.GetAllByBrandIdAsync(request.BrandId);
            if (orderItems == null)
                return new Response<IEnumerable<OrderItemDto>>("Order items not found");

            var orderItemDtos = _mapper.Map<IEnumerable<OrderItemDto>>(orderItems);
            return new Response<IEnumerable<OrderItemDto>>(orderItemDtos, "Success");
        }
    }
}
