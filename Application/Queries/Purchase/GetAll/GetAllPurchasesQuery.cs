using Application.Bases;
using Application.Dtos.Purchasing;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.Purchase.GetAll
{
    public class GetAllPurchasesQuery : IRequest<Response<IEnumerable<PurchaseDto>>>
    {
    }

    public class GetAllPurchasesQueryHandler : BaseHandler<IPurchaseCommandRepository>, IRequestHandler<GetAllPurchasesQuery, Response<IEnumerable<PurchaseDto>>>
    {
        public GetAllPurchasesQueryHandler(IPurchaseCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<IEnumerable<PurchaseDto>>> Handle(GetAllPurchasesQuery request, CancellationToken cancellationToken)
        {
            var items = await _repo.GetAllTableAsync();
            var dtos = _mapper.Map<IEnumerable<PurchaseDto>>(items);
            return new Response<IEnumerable<PurchaseDto>>(dtos, "Success");
        }
    }
}
