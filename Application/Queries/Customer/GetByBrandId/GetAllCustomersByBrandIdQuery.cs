using Application.Bases;
using Application.Dtos.Core;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Customer.GetByBrandId
{
    public class GetAllCustomersByBrandIdQuery : IRequest<Response<IEnumerable<CustomerDto>>>
    {
        public Guid BrandId { get; set; }

        public GetAllCustomersByBrandIdQuery(Guid brandId)
        {
            BrandId = brandId;
        }
    }

    public class GetAllCustomersByBrandIdQueryHandler : IRequestHandler<GetAllCustomersByBrandIdQuery, Response<IEnumerable<CustomerDto>>>
    {
        private readonly ICustomerQueryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllCustomersByBrandIdQueryHandler(ICustomerQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<CustomerDto>>> Handle(GetAllCustomersByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var customers = await _repository.GetAllByBrandIdAsync(request.BrandId);
            if (customers == null)
                return new Response<IEnumerable<CustomerDto>>("Customers not found");

            var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);
            return new Response<IEnumerable<CustomerDto>>(customerDtos, "Success");
        }
    }
}
