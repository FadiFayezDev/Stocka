using Application.Bases;
using Application.Dtos.Orders;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.OrderItem.GetAll
{
    public class GetAllOrderItemsQuery : IRequest<Response<IEnumerable<OrderItemDto>>>
    {
    }

    public class GetAllOrderItemsQueryHandler : BaseHandler<IOrderItemCommandRepository>, IRequestHandler<GetAllOrderItemsQuery, Response<IEnumerable<OrderItemDto>>>
    {
        public GetAllOrderItemsQueryHandler(IOrderItemCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<IEnumerable<OrderItemDto>>> Handle(GetAllOrderItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await _repo.GetAllTableAsync();
            var dtos = _mapper.Map<IEnumerable<OrderItemDto>>(items);
            return new Response<IEnumerable<OrderItemDto>>(dtos, "Success");
        }
    }
}
