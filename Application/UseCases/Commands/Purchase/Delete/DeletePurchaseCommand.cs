using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Purchasing;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Purchase.Delete
{
    public class DeletePurchaseCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeletePurchaseCommandHandler : BaseHandler<IPurchaseCommandRepository>, IRequestHandler<DeletePurchaseCommand, Response<bool>>
    {
        public DeletePurchaseCommandHandler(IPurchaseCommandRepository repository, IUnitOfWork unitOfWork)
            : base(null, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeletePurchaseCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>(false, "Purchase not found");

            return await ExecuteDeleteAsync(
                existing,
                async (p) => await _repo.DeleteAsync(p),
                cancellationToken);
        }
    }
}
