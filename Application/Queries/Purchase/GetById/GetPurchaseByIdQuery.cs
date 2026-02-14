using Application.Bases;
using Application.Dtos.Purchasing;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.Purchase.GetById
{
    public class GetPurchaseByIdQuery : IRequest<Response<PurchaseDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetPurchaseByIdQueryHandler : BaseHandler<IPurchaseCommandRepository>, IRequestHandler<GetPurchaseByIdQuery, Response<PurchaseDto>>
    {
        public GetPurchaseByIdQueryHandler(IPurchaseCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<PurchaseDto>> Handle(GetPurchaseByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetByIdAsync(request.Id);
            if (item == null)
                return new Response<PurchaseDto>("Not found");

            var dto = _mapper.Map<PurchaseDto>(item);
            return new Response<PurchaseDto>(dto, "Success");
        }
    }
}
