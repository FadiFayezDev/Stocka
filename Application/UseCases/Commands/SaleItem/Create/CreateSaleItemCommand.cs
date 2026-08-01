using Application.Bases;
using Application.Common.Interfaces;
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
        public CreateOrderItemCommandHandler(IOrderItemCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<OrderItemDto>> Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Orders.OrderItem>(request);

            return await ExecuteCreateAsync<Domain.Entities.Orders.OrderItem, OrderItemDto>(
                entity,
                async (oi) => await _repo.CreateAsync(oi),
                cancellationToken);
        }
    }
}
