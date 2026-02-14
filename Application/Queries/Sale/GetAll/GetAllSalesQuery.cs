using Application.Bases;
using Application.Dtos.Sales;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Sale.GetAll
{
    public class GetAllSalesQuery : IRequest<Response<IEnumerable<SaleDto>>>
    {
    }

    public class GetAllSalesQueryHandler : IRequestHandler<GetAllSalesQuery, Response<IEnumerable<SaleDto>>>
    {
        private readonly ISaleQueryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllSalesQueryHandler(ISaleQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<SaleDto>>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetAllTableAsync();
            var dtos = _mapper.Map<IEnumerable<SaleDto>>(items);
            return new Response<IEnumerable<SaleDto>>(dtos, "Success");
        }
    }
}
