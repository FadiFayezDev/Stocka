using Application.Bases;
using Application.Dtos.Purchasing;
using AutoMapper;
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
        public DeletePurchaseItemCommandHandler(IPurchaseItemCommandRepository Repository) : base(null, Repository)
        {
        }

        public async Task<Response<bool>> Handle(DeletePurchaseItemCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>("Not found");

            await _repo.DeleteAsync(existing);

            return new Response<bool>();
        }
    }
}
