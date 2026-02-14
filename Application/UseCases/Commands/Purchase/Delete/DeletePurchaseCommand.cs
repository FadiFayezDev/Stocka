using Application.Bases;
using Application.Dtos.Purchasing;
using AutoMapper;
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
        public DeletePurchaseCommandHandler(IPurchaseCommandRepository Repository) : base(null, Repository)
        {
        }

        public async Task<Response<bool>> Handle(DeletePurchaseCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>("Not found");

            await _repo.DeleteAsync(existing);

            return new Response<bool>();
        }
    }
}
