using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Purchasing;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.UseCases.Purchase
{
    public class VoidPurchaseCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class VoidPurchaseCommandHandler : BaseHandler<IPurchaseCommandRepository>, IRequestHandler<VoidPurchaseCommand, Response<bool>>
    {
        public VoidPurchaseCommandHandler(IPurchaseCommandRepository repository, IUnitOfWork unitOfWork)
            : base(null, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(VoidPurchaseCommand request, CancellationToken cancellationToken)
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
