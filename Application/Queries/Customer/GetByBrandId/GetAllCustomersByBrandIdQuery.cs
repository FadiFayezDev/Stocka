using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Core;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Customer.GetByBrandId
{
    /// <summary>
    /// The brand ID is injected automatically.
    /// </summary>
    public class GetAllCustomersByBrandIdQuery : IRequest<Response<IEnumerable<CustomerDto>>>
    {

    }

    public class GetAllCustomersByBrandIdQueryHandler : IRequestHandler<GetAllCustomersByBrandIdQuery, Response<IEnumerable<CustomerDto>>>
    {
        private readonly ICustomerQueryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUser;

        public GetAllCustomersByBrandIdQueryHandler(ICustomerQueryRepository repository, IMapper mapper, ICurrentUserContext currentUser)
        {
            _repository = repository;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<Response<IEnumerable<CustomerDto>>> Handle(GetAllCustomersByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var customers = await _repository.GetAllByBrandIdAsync(brandId);
            if (customers == null)
                return new Response<IEnumerable<CustomerDto>>("Customers not found");

            var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);
            return new Response<IEnumerable<CustomerDto>>(customerDtos, "Success");
        }
    }
}