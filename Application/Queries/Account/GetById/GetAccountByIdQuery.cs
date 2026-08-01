using Application.Bases;
using Application.Dtos.Accounting;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Account.GetById
{
    public class GetAccountByIdQuery : IRequest<Response<AccountDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, Response<AccountDto>>
    {
        private readonly IAccountQueryRepository _queryRepository;
        private readonly IMapper _mapper;

        public GetAccountByIdQueryHandler(IAccountQueryRepository queryRepository, IMapper mapper)
        {
            _queryRepository = queryRepository;
            _mapper = mapper;
        }

        public async Task<Response<AccountDto>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _queryRepository.GetByIdAsync(request.Id);
            if (item == null)
                return new Response<AccountDto>("Not found");

            return new Response<AccountDto>(item, "Success");
        }
    }
}
