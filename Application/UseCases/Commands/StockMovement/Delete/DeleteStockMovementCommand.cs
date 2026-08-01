using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Products;
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
        public DeleteStockMovementCommandHandler(IStockMovementCommandRepository repository, IUnitOfWork unitOfWork)
            : base(null, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeleteStockMovementCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>(false, "Stock movement not found");

            return await ExecuteDeleteAsync(
                existing,
                async (sm) => await _repo.DeleteAsync(sm),
                cancellationToken);
        }
    }
}
