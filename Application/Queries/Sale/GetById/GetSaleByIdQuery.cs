using Application.Bases;
using Application.Dtos.Sales;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.Sale.GetById
{
    public class GetSaleByIdQuery : IRequest<Response<SaleDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetSaleByIdQueryHandler : BaseHandler<ISaleCommandRepository>, IRequestHandler<GetSaleByIdQuery, Response<SaleDto>>
    {
        public GetSaleByIdQueryHandler(ISaleCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<SaleDto>> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetByIdAsync(request.Id);
            if (item == null)
                return new Response<SaleDto>("Not found");

            var dto = _mapper.Map<SaleDto>(item);
            return new Response<SaleDto>(dto, "Success");
        }
    }
}
