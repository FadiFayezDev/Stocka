using Application.Bases;
using Application.Dtos.Sales;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.SaleItem.GetById
{
    public class GetSaleItemByIdQuery : IRequest<Response<SaleItemDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetSaleItemByIdQueryHandler : BaseHandler<ISaleItemCommandRepository>, IRequestHandler<GetSaleItemByIdQuery, Response<SaleItemDto>>
    {
        public GetSaleItemByIdQueryHandler(ISaleItemCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<SaleItemDto>> Handle(GetSaleItemByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetByIdAsync(request.Id);
            if (item == null)
                return new Response<SaleItemDto>("Not found");

            var dto = _mapper.Map<SaleItemDto>(item);
            return new Response<SaleItemDto>(dto, "Success");
        }
    }
}
