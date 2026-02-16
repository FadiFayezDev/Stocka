using Application.Bases;
using Application.Dtos.Orders;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.OrderItem.Update
{
    public class UpdateOrderItemCommand : IRequest<Response<OrderItemDto>>
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Guid BatchId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal CostPrice { get; set; }
    }

    public class UpdateOrderItemCommandHandler : BaseHandler<IOrderItemCommandRepository>, IRequestHandler<UpdateOrderItemCommand, Response<OrderItemDto>>
    {
        public UpdateOrderItemCommandHandler(IOrderItemCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<OrderItemDto>> Handle(UpdateOrderItemCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<OrderItemDto>("Not found");

            _mapper.Map(request, existing);

            await _repo.UpdateAsync(existing);

            return new Response<OrderItemDto>();
        }
    }
}
