using Application.Bases;
using Application.Common.Interfaces;
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
        public CreatePurchaseItemCommandHandler(IPurchaseItemCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<PurchaseItemDto>> Handle(CreatePurchaseItemCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Purchasing.PurchaseItem>(request);

            return await ExecuteCreateAsync<Domain.Entities.Purchasing.PurchaseItem, PurchaseItemDto>(
                entity,
                async (pi) => await _repo.CreateAsync(pi),
                cancellationToken);
        }
    }
}
