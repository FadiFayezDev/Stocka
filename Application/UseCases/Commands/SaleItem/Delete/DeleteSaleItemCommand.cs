using Application.Bases;
using Application.Dtos.Orders;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.OrderItem.Delete
{
    public class DeleteOrderItemCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteOrderItemCommandHandler : BaseHandler<IOrderItemCommandRepository>, IRequestHandler<DeleteOrderItemCommand, Response<bool>>
    {
        public DeleteOrderItemCommandHandler(IOrderItemCommandRepository Repository) : base(null, Repository)
        {
        }

        public async Task<Response<bool>> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>("Not found");

            await _repo.DeleteAsync(existing);

            return new Response<bool>();
        }
    }
}
