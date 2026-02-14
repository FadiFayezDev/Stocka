using Application.Bases;
using Application.Dtos.Sales;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.SaleItem.GetAll
{
    public class GetAllSaleItemsQuery : IRequest<Response<IEnumerable<SaleItemDto>>>
    {
    }

    public class GetAllSaleItemsQueryHandler : BaseHandler<ISaleItemCommandRepository>, IRequestHandler<GetAllSaleItemsQuery, Response<IEnumerable<SaleItemDto>>>
    {
        public GetAllSaleItemsQueryHandler(ISaleItemCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<IEnumerable<SaleItemDto>>> Handle(GetAllSaleItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await _repo.GetAllTableAsync();
            var dtos = _mapper.Map<IEnumerable<SaleItemDto>>(items);
            return new Response<IEnumerable<SaleItemDto>>(dtos, "Success");
        }
    }
}
