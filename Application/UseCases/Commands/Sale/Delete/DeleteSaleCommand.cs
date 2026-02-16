using Application.Bases;
using Application.Dtos.Orders;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Order.Delete
{
    public class DeleteOrderCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteOrderCommandHandler : BaseHandler<IOrderCommandRepository>, IRequestHandler<DeleteOrderCommand, Response<bool>>
    {
        public DeleteOrderCommandHandler(IOrderCommandRepository Repository) : base(null, Repository)
        {
        }

        public async Task<Response<bool>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>("Not found");

            await _repo.DeleteAsync(existing);

            return new Response<bool>();
        }
    }
}
