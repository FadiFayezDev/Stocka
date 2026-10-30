using BuildingBlocks.Application.Common.Interfaces;
using Identity.Application.Repositories.Queries;
using Identity.Contracts.DTOs;
using MediatR;

namespace Identity.Application.UseCases.Customers
{
    internal class RetrieveCustomerByNameQuery : IRequest<IEnumerable<CustomerDto>>
    {
        public string Name { get; set; } = default!;
    }

    internal class RetrieveCustomerByNameQueryHandler : IRequestHandler<RetrieveCustomerByNameQuery, IEnumerable<CustomerDto>>
    {
        private readonly ICustomerQueryRepository _customerQuery;
        private readonly ICurrentUserContext _currentUserContext;

        public RetrieveCustomerByNameQueryHandler(ICustomerQueryRepository customerQuery, ICurrentUserContext currentUserContext)
        {
            _customerQuery = customerQuery;
            _currentUserContext = currentUserContext;
        }
        public async Task<IEnumerable<CustomerDto>> Handle(RetrieveCustomerByNameQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(request.Name));

            var organizationId = _currentUserContext.ActiveOrganizationId;
            return await _customerQuery.GetByNameAsync(organizationId, request.Name);
        }
    }
}