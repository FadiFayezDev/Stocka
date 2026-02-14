using Application.Bases;
using Application.Dtos.Purchasing;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.PurchaseItem.GetAll
{
    public class GetAllPurchaseItemsQuery : IRequest<Response<IEnumerable<PurchaseItemDto>>>
    {
    }

    public class GetAllPurchaseItemsQueryHandler : BaseHandler<IPurchaseItemCommandRepository>, IRequestHandler<GetAllPurchaseItemsQuery, Response<IEnumerable<PurchaseItemDto>>>
    {
        public GetAllPurchaseItemsQueryHandler(IPurchaseItemCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<IEnumerable<PurchaseItemDto>>> Handle(GetAllPurchaseItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await _repo.GetAllTableAsync();
            var dtos = _mapper.Map<IEnumerable<PurchaseItemDto>>(items);
            return new Response<IEnumerable<PurchaseItemDto>>(dtos, "Success");
        }
    }
}
