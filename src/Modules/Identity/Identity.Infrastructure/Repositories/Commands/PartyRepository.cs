using Identity.Application.Repositories.Commands;
using Identity.Domain.Parties;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Repositories.Commands.Bases;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories.Commands
{
    /// <summary>
    /// Write-side repository for the Party aggregate.
    /// <para>
    /// The aggregate consistency rule: a <c>Party</c> must always load together with its
    /// <c>PartyOrganizations</c> — membership mutations (<c>AssignOrganization</c> /
    /// <c>UnassignOrganization</c>) operate on the in-memory collection.
    /// </para>
    /// </summary>
    internal sealed class PartyRepository : Repository<Party, PartyId>, IPartyRepository
    {
        public PartyRepository(AppIdentityDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Always returns the full aggregate (root + organizations), so callers never
        /// accidentally work against a partially loaded Party.
        /// </summary>
        public override async Task<Party?> GetByIdAsync(PartyId id)
        {
            return await Set
                .Include(p => p.PartyOrganizations)
                .FirstOrDefaultAsync(p => p.Id.Equals(id));
        }

        /// <summary>
        /// Explicit intent-named variant of <see cref="GetByIdAsync"/>.
        /// Kept separate on the contract so calling code reads clearly at the use-case level.
        /// </summary>
        public Task<Party?> GetWithOrganizationsAsync(PartyId partyId)
            => GetByIdAsync(partyId);

        public async Task<bool> ExistsAsync(PartyId partyId)
            => await Set.AnyAsync(p => p.Id.Equals(partyId));
    }
}
