using Application.Bases;
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
        public UpdateOrderCommandHandler(IOrderCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<OrderDto>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<OrderDto>("Not found");

            _mapper.Map(request, existing);

           await _repo.UpdateAsync(existing);
            return new Response<OrderDto>();
        }
    }
}
