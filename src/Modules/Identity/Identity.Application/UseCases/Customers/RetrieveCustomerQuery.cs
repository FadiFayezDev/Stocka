using BuildingBlocks.Application.Common.Interfaces;
using Identity.Application.Repositories.Queries;
using Identity.Contracts.DTOs;
using Identity.Domain.Parties;
using MediatR;

namespace Identity.Application.UseCases.Customers
{
    internal class RetrieveCustomerQuery : IRequest<CustomerDto?>
    {
        public Guid CustomerId { get; set; }
    }

    internal class RetrieveCustomerQueryHandler : IRequestHandler<RetrieveCustomerQuery, CustomerDto?>
    {
        private readonly ICustomerQueryRepository _customerQuery;
        private readonly ICurrentUserContext _currentUserContext;

        public RetrieveCustomerQueryHandler(ICustomerQueryRepository customerQuery, ICurrentUserContext currentUserContext)
        {
            _customerQuery = customerQuery;
            _currentUserContext = currentUserContext;
        }

        public async Task<CustomerDto?> Handle(RetrieveCustomerQuery request, CancellationToken cancellationToken)
        {
            if (request.CustomerId == Guid.Empty)
                throw new ArgumentException("CustomerId cannot be empty.", nameof(request.CustomerId));

            var organizationId = _currentUserContext.ActiveOrganizationId;
            return await _customerQuery.GetByIdAsync(PartyId.From(request.CustomerId));
        }
    }
}