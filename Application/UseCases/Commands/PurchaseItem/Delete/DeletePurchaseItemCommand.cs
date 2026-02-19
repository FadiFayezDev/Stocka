using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Purchasing;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.PurchaseItem.Delete
{
    public class DeletePurchaseItemCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeletePurchaseItemCommandHandler : BaseHandler<IPurchaseItemCommandRepository>, IRequestHandler<DeletePurchaseItemCommand, Response<bool>>
    {
        public DeletePurchaseItemCommandHandler(IPurchaseItemCommandRepository repository, IUnitOfWork unitOfWork)
            : base(null, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeletePurchaseItemCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>(false, "Purchase item not found");

            return await ExecuteDeleteAsync(
                existing,
                async (pi) => await _repo.DeleteAsync(pi),
                cancellationToken);
        }
    }
}
