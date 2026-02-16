using Application.Bases;
using Application.Dtos.Orders;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.OrderItem.Create
{
    public class CreateOrderItemCommand : IRequest<Response<OrderItemDto>>
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Guid BatchId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal CostPrice { get; set; }
    }

    public class CreateOrderItemCommandHandler : BaseHandler<IOrderItemCommandRepository>, IRequestHandler<CreateOrderItemCommand, Response<OrderItemDto>>
    {
        public CreateOrderItemCommandHandler(IOrderItemCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<OrderItemDto>> Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Orders.OrderItem>(request);

            await _repo.CreateAsync(entity);

            return new Response<OrderItemDto>();
        }
    }
}
