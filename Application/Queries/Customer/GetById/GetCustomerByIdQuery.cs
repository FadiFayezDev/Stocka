using Application.Bases;
using Application.Dtos.Core;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Customer.GetById
{
    public class GetCustomerByIdQuery : IRequest<Response<CustomerDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Response<CustomerDto>>
    {
        private readonly ICustomerQueryRepository _queryRepository;
        private readonly IMapper _mapper;

        public GetCustomerByIdQueryHandler(ICustomerQueryRepository queryRepository, IMapper mapper)
        {
            _queryRepository = queryRepository;
            _mapper = mapper;
        }

        public async Task<Response<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _queryRepository.GetByIdAsync(request.Id);
            if (customer == null)
                return new Response<CustomerDto>("Customer not found");

            return new Response<CustomerDto>(customer, "Success");
        }
    }
}
