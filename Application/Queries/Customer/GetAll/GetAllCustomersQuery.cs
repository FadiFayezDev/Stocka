using Application.Bases;
using Application.Dtos.Core;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Customer.GetAll
{
    public class GetAllCustomersQuery : IRequest<Response<IEnumerable<CustomerDto>>>
    {
    }

    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, Response<IEnumerable<CustomerDto>>>
    {
        private readonly ICustomerQueryRepository _queryRepository;
        private readonly IMapper _mapper;

        public GetAllCustomersQueryHandler(ICustomerQueryRepository queryRepository, IMapper mapper)
        {
            _queryRepository = queryRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<CustomerDto>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _queryRepository.GetAllTableAsync();
            var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);
            return new Response<IEnumerable<CustomerDto>>(customerDtos, "Success");
        }
    }
}
