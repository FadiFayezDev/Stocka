using BuildingBlocks.Application.Common.Interfaces;
using Identity.Application.Common.Interfaces;
using Identity.Application.Repositories.Commands;
using Identity.Domain.Parties;
using MediatR;

namespace Identity.Application.UseCases.Customers;

internal sealed record AddCustomerLoyaltyPointsCommand(
    Guid CustomerId,
    int Points) : ITransactionCommand;

internal sealed class AddCustomerLoyaltyPointsCommandHandler
    : IRequestHandler<AddCustomerLoyaltyPointsCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IPartyRepository _partyRepository;
    private readonly BuildingBlocks.Application.Common.Interfaces.ICurrentUserContext _currentUserContext;

    public AddCustomerLoyaltyPointsCommandHandler(
        ICustomerRepository customerRepository,
        IPartyRepository partyRepository,
        BuildingBlocks.Application.Common.Interfaces.ICurrentUserContext currentUserContext)
    {
        _customerRepository = customerRepository;
        _partyRepository = partyRepository;
        _currentUserContext = currentUserContext;
    }

    public async Task Handle(
        AddCustomerLoyaltyPointsCommand request,
        CancellationToken cancellationToken)
    {
        Validate(request.CustomerId, request.Points, allowZero: false);
        var customer = await GetCustomerAsync(request.CustomerId);
        customer.AddLoyaltyPoint(request.Points);
        await _customerRepository.UpdateAsync(customer);
    }

    private async Task<Domain.Customer.Customer> GetCustomerAsync(Guid customerId)
    {
        var id = PartyId.From(customerId);
        var party = await _partyRepository.GetWithOrganizationsAsync(id);
        var customer = await _customerRepository.GetByIdAsync(id);

        if (party is null || customer is null || !party.PartyOrganizations.Any(
                membership => membership.Organization == _currentUserContext.ActiveOrganizationId))
        {
            throw new KeyNotFoundException($"Customer '{customerId}' was not found in the active organization.");
        }

        return customer;
    }

    private static void Validate(Guid customerId, int points, bool allowZero)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("CustomerId cannot be empty.", nameof(customerId));

        if (allowZero ? points < 0 : points <= 0)
            throw new ArgumentException("Points must be greater than zero.", nameof(points));
    }
}
