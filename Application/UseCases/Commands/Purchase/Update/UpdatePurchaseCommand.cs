using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Purchasing;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Purchase.Update
{
    public class UpdatePurchaseCommand : IRequest<Response<PurchaseDto>>
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public Guid SupplierId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class UpdatePurchaseCommandHandler : BaseHandler<IPurchaseCommandRepository>, IRequestHandler<UpdatePurchaseCommand, Response<PurchaseDto>>
    {
        public UpdatePurchaseCommandHandler(IPurchaseCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<PurchaseDto>> Handle(UpdatePurchaseCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<PurchaseDto>("Purchase not found");

            _mapper.Map(request, existing);

            return await ExecuteUpdateAsync<Domain.Entities.Purchasing.Purchase, PurchaseDto>(
                existing,
                async (p) => await _repo.UpdateAsync(p),
                cancellationToken);
        }
    }
}