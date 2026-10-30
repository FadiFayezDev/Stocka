using BuildingBlocks.Application.Common.Interfaces;
using Identity.Application.Common.Interfaces;
using Identity.Application.Repositories.Commands;
using Identity.Domain.Parties;
using MediatR;

namespace Identity.Application.UseCases.Customers;

internal sealed record UpdateCustomerLoyaltyPointsCommand(
    Guid CustomerId,
    int Points) : ITransactionCommand;

internal sealed class UpdateCustomerLoyaltyPointsCommandHandler
    : IRequestHandler<UpdateCustomerLoyaltyPointsCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IPartyRepository _partyRepository;
    private readonly BuildingBlocks.Application.Common.Interfaces.ICurrentUserContext _currentUserContext;

    public UpdateCustomerLoyaltyPointsCommandHandler(
        ICustomerRepository customerRepository,
        IPartyRepository partyRepository,
        BuildingBlocks.Application.Common.Interfaces.ICurrentUserContext currentUserContext)
    {
        _customerRepository = customerRepository;
        _partyRepository = partyRepository;
        _currentUserContext = currentUserContext;
    }

    public async Task Handle(
        UpdateCustomerLoyaltyPointsCommand request,
        CancellationToken cancellationToken)
    {
        if (request.CustomerId == Guid.Empty)
            throw new ArgumentException("CustomerId cannot be empty.", nameof(request.CustomerId));

        if (request.Points < 0)
            throw new ArgumentException("Points cannot be negative.", nameof(request.Points));

        var id = PartyId.From(request.CustomerId);
        var party = await _partyRepository.GetWithOrganizationsAsync(id);
        var customer = await _customerRepository.GetByIdAsync(id);

        if (party is null || customer is null || !party.PartyOrganizations.Any(
                membership => membership.Organization == _currentUserContext.ActiveOrganizationId))
        {
            throw new KeyNotFoundException(
                $"Customer '{request.CustomerId}' was not found in the active organization.");
        }

        customer.UpdateLoyaltyPoint(request.Points);
        await _customerRepository.UpdateAsync(customer);
    }
}
