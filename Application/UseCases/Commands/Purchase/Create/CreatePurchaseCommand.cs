using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Purchasing;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Purchase.Create
{
    public class CreatePurchaseCommand : IRequest<Response<PurchaseDto>>
    {
        public Guid BrandId { get; set; }
        public Guid SupplierId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class CreatePurchaseCommandHandler : BaseHandler<IPurchaseCommandRepository>, IRequestHandler<CreatePurchaseCommand, Response<PurchaseDto>>
    {
        public CreatePurchaseCommandHandler(IPurchaseCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<PurchaseDto>> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Purchasing.Purchase>(request);

            return await ExecuteCreateAsync<Domain.Entities.Purchasing.Purchase, PurchaseDto>(
                entity,
                async (p) => await _repo.CreateAsync(p),
                cancellationToken);
        }
    }
}
