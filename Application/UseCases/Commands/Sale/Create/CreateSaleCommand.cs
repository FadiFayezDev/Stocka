using Application.Bases;
using Application.Common.Interfaces;
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
        public CreateOrderCommandHandler(IOrderCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Orders.Order>(request);

            return await ExecuteCreateAsync<Domain.Entities.Orders.Order, OrderDto>(
                entity,
                async (o) => await _repo.CreateAsync(o),
                cancellationToken);
        }
    }
}
