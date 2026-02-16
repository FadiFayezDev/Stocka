using Application.Bases;
using Application.Dtos.Orders;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Order.Create
{
    public class CreateOrderCommand : IRequest<Response<OrderDto>>
    {
        public Guid BrandId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
    }

    public class CreateOrderCommandHandler : BaseHandler<IOrderCommandRepository>, IRequestHandler<CreateOrderCommand, Response<OrderDto>>
    {
        public CreateOrderCommandHandler(IOrderCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Orders.Order>(request);

            await _repo.CreateAsync(entity);
            return new Response<OrderDto>();
        }
    }
}
