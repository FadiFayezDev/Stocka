using Application.Bases;
using Application.Dtos.Purchasing;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.PurchaseItem.GetById
{
    public class GetPurchaseItemByIdQuery : IRequest<Response<PurchaseItemDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetPurchaseItemByIdQueryHandler : BaseHandler<IPurchaseItemCommandRepository>, IRequestHandler<GetPurchaseItemByIdQuery, Response<PurchaseItemDto>>
    {
        public GetPurchaseItemByIdQueryHandler(IPurchaseItemCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<PurchaseItemDto>> Handle(GetPurchaseItemByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetByIdAsync(request.Id);
            if (item == null)
                return new Response<PurchaseItemDto>("Not found");

            var dto = _mapper.Map<PurchaseItemDto>(item);
            return new Response<PurchaseItemDto>(dto, "Success");
        }
    }
}
