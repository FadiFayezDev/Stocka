using BuildingBlocks.Application.Common.Models;
using Identity.Contracts.DTOs;
using Identity.Domain.Parties;

namespace Identity.Application.Repositories.Queries
{
    /// <summary>Read-side (CQRS) queries over the Parties supertype table (the shared person table).</summary>
    public interface IPartyQueryRepository
    {
        /// <summary>Single party with its organization memberships.</summary>
        Task<PartyDto?> GetByIdAsync(PartyId id);

        /// <summary>Exact phone-number match within the organization.</summary>
        Task<PartyDto?> GetByPhoneNumberAsync(Guid organizationId, string phoneNumber);

        /// <summary>All parties belonging to the organization (through their PartyOrganizations).</summary>
        Task<IEnumerable<PartyDto>> GetAllAsync(Guid organizationId);

        Task<PaginatedResult<PartyDto>> GetPagedAsync(Guid organizationId, int page, int pageSize);
    }
}
