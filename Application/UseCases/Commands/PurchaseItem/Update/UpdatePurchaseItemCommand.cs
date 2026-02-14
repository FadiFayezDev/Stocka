using Application.Bases;
using Application.Dtos.Purchasing;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.PurchaseItem.Update
{
    public class UpdatePurchaseItemCommand : IRequest<Response<PurchaseItemDto>>
    {
        public Guid Id { get; set; }
        public Guid PurchaseId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
    }

    public class UpdatePurchaseItemCommandHandler : BaseHandler<IPurchaseItemCommandRepository>, IRequestHandler<UpdatePurchaseItemCommand, Response<PurchaseItemDto>>
    {
        public UpdatePurchaseItemCommandHandler(IPurchaseItemCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<PurchaseItemDto>> Handle(UpdatePurchaseItemCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<PurchaseItemDto>("Not found");

            _mapper.Map(request, existing);

             await _repo.UpdateAsync(existing);
            return new Response<PurchaseItemDto>();
        }
    }
}