using Application.Bases;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.StockMovement.Delete
{
    public class DeleteStockMovementCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteStockMovementCommandHandler : BaseHandler<IStockMovementCommandRepository>, IRequestHandler<DeleteStockMovementCommand, Response<bool>>
    {
        public DeleteStockMovementCommandHandler(IStockMovementCommandRepository Repository) : base(null, Repository)
        {
        }

        public async Task<Response<bool>> Handle(DeleteStockMovementCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>("Not found");

            await _repo.DeleteAsync(existing);

            return new Response<bool>();
        }
    }
}
