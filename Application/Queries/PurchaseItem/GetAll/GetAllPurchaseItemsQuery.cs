using Application.Bases;
using Application.Common.Interfaces;
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
        private readonly ICurrentUserContext _currentUser;
        public GetAllPurchaseItemsQueryHandler(IPurchaseItemCommandRepository Repository, IMapper mapper, ICurrentUserContext currentUser) : base(mapper, Repository)
        {
            _currentUser = currentUser;
        }

        public async Task<Response<IEnumerable<PurchaseItemDto>>> Handle(GetAllPurchaseItemsQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;
            var items = await _repo.GetAllTableAsync();
            var dtos = _mapper.Map<IEnumerable<PurchaseItemDto>>(items);
            return new Response<IEnumerable<PurchaseItemDto>>(dtos, "Success");
        }
    }
}