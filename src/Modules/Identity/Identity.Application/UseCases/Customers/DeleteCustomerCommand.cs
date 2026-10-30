using Identity.Application.Common.Interfaces;
using Identity.Application.Repositories.Commands;
using Identity.Domain.Parties;
using MediatR;

namespace Identity.Application.UseCases.Customers;

internal sealed record DeleteCustomerCommand(Guid CustomerId) : ITransactionCommand;

internal sealed class DeleteCustomerCommandHandler
    : IRequestHandler<DeleteCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IPartyRepository _partyRepository;
    private readonly BuildingBlocks.Application.Common.Interfaces.ICurrentUserContext _currentUserContext;

    public DeleteCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IPartyRepository partyRepository,
        BuildingBlocks.Application.Common.Interfaces.ICurrentUserContext currentUserContext)
    {
        _customerRepository = customerRepository;
        _partyRepository = partyRepository;
        _currentUserContext = currentUserContext;
    }

    public async Task Handle(
        DeleteCustomerCommand request,
        CancellationToken cancellationToken)
    {
        if (request.CustomerId == Guid.Empty)
            throw new ArgumentException("CustomerId cannot be empty.", nameof(request.CustomerId));

        var customerId = PartyId.From(request.CustomerId);
        var party = await _partyRepository.GetWithOrganizationsAsync(customerId);

        if (party is null || !party.PartyOrganizations.Any(
                membership => membership.Organization == _currentUserContext.ActiveOrganizationId))
        {
            throw new KeyNotFoundException(
                $"Customer '{request.CustomerId}' was not found in the active organization.");
        }

        var customer = await _customerRepository.GetByIdAsync(customerId);

        if (customer is null)
            throw new KeyNotFoundException($"Customer '{request.CustomerId}' was not found.");

        await _customerRepository.RemoveAsync(customer);
    }
}
