using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Orders;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Order.Update
{
    public class UpdateOrderCommand : IRequest<Response<OrderDto>>
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
    }

    public class UpdateOrderCommandHandler : BaseHandler<IOrderCommandRepository>, IRequestHandler<UpdateOrderCommand, Response<OrderDto>>
    {
        public UpdateOrderCommandHandler(IOrderCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<OrderDto>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<OrderDto>("Order not found");

            _mapper.Map(request, existing);

            return await ExecuteUpdateAsync<Domain.Entities.Orders.Order, OrderDto>(
                existing,
                async (o) => await _repo.UpdateAsync(o),
                cancellationToken);
        }
    }
}
