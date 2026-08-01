using Application.Bases;
using Application.Dtos.Accounting;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Account.GetAll
{
    public class GetAllAccountsQuery : IRequest<Response<IEnumerable<AccountDto>>>
    {
    }

    public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, Response<IEnumerable<AccountDto>>>
    {
        private readonly IAccountQueryRepository _queryRepository;
        private readonly IMapper _mapper;

        public GetAllAccountsQueryHandler(IAccountQueryRepository queryRepository, IMapper mapper)
        {
            _queryRepository = queryRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<AccountDto>>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
        {
            var items = await _queryRepository.GetAllTableAsync();
            var dtos = _mapper.Map<IEnumerable<AccountDto>>(items);
            return new Response<IEnumerable<AccountDto>>(dtos, "Success");
        }
    }
}
