using BuildingBlocks.Application.Common.Interfaces;
using Identity.Domain.Parties;

namespace Identity.Application.Repositories.Commands
{
    /// <summary>
    /// Write-side repository for the Party aggregate.
    /// <para>
    /// A Party is the supertype row shared by Customer/Employee/Supplier
    /// (their ids ARE this Party's id). Note that role tables should always
    /// be filled from an <c>existing</c> Party, never with a fresh generated id.
    /// </para>
    /// </summary>
    public interface IPartyRepository : IRepository<Party, PartyId>
    {
        /// <summary>
        /// Loads the aggregate including its <c>PartyOrganizations</c>.
        /// Required before calling <c>AssignOrganization</c>/<c>UnassignOrganization</c>,
        /// because membership mutations operate on the in-memory collection.
        /// </summary>
        Task<Party?> GetWithOrganizationsAsync(PartyId partyId);

        /// <summary>True when a Party with this id exists (cheap existence check for role creation flows).</summary>
        Task<bool> ExistsAsync(PartyId partyId);
    }
}
