using Application.Bases;
using Application.Dtos.Purchasing;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.PurchaseItem.Create
{
    public class CreatePurchaseItemCommand : IRequest<Response<PurchaseItemDto>>
    {
        public Guid PurchaseId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
    }

    public class CreatePurchaseItemCommandHandler : BaseHandler<IPurchaseItemCommandRepository>, IRequestHandler<CreatePurchaseItemCommand, Response<PurchaseItemDto>>
    {
        public CreatePurchaseItemCommandHandler(IPurchaseItemCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<PurchaseItemDto>> Handle(CreatePurchaseItemCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Purchasing.PurchaseItem>(request);

            await _repo.CreateAsync(entity);
            return new Response<PurchaseItemDto>();
        }
    }
}
