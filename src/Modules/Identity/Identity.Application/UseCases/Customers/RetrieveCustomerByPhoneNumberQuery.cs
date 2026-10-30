using BuildingBlocks.Application.Common.Interfaces;
using Identity.Application.Repositories.Queries;
using Identity.Contracts.DTOs;
using MediatR;

namespace Identity.Application.UseCases.Customers;

internal sealed record RetrieveCustomerByPhoneNumberQuery(string PhoneNumber)
    : IRequest<CustomerDto?>;

internal sealed class RetrieveCustomerByPhoneNumberQueryHandler
    : IRequestHandler<RetrieveCustomerByPhoneNumberQuery, CustomerDto?>
{
    private readonly ICustomerQueryRepository _customerQuery;
    private readonly ICurrentUserContext _currentUserContext;

    public RetrieveCustomerByPhoneNumberQueryHandler(
        ICustomerQueryRepository customerQuery,
        ICurrentUserContext currentUserContext)
    {
        _customerQuery = customerQuery;
        _currentUserContext = currentUserContext;
    }

    public async Task<CustomerDto?> Handle(
        RetrieveCustomerByPhoneNumberQuery request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            throw new ArgumentException("Phone number cannot be empty.", nameof(request.PhoneNumber));

        return await _customerQuery.GetByPhoneNumberAsync(
            _currentUserContext.ActiveOrganizationId,
            request.PhoneNumber.Trim());
    }
}
