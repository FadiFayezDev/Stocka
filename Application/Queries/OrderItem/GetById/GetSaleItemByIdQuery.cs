using Application.Bases;
using Application.Dtos.Orders;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.OrderItem.GetById
{
    public class GetOrderItemByIdQuery : IRequest<Response<OrderItemDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetOrderItemByIdQueryHandler : BaseHandler<IOrderItemCommandRepository>, IRequestHandler<GetOrderItemByIdQuery, Response<OrderItemDto>>
    {
        public GetOrderItemByIdQueryHandler(IOrderItemCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<OrderItemDto>> Handle(GetOrderItemByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetByIdAsync(request.Id);
            if (item == null)
                return new Response<OrderItemDto>("Not found");

            var dto = _mapper.Map<OrderItemDto>(item);
            return new Response<OrderItemDto>(dto, "Success");
        }
    }
}
