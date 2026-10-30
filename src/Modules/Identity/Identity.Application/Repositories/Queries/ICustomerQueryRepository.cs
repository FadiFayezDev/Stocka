using BuildingBlocks.Application.Common.Models;
using Identity.Contracts.DTOs;
using Identity.Domain.Parties;

namespace Identity.Application.Repositories.Queries
{
    /// <summary>Read-side (CQRS) queries for Customer role rows, projected to their contract DTOs.</summary>
    public interface ICustomerQueryRepository
    {
        /// <summary>Single customer by the shared key (which is the owning Party's id).</summary>
        Task<CustomerDto?> GetByIdAsync(PartyId id);

        /// <summary>Exact phone-number match within the organization (Party phone).</summary>
        Task<CustomerDto?> GetByPhoneNumberAsync(Guid organizationId, string phoneNumber);

        /// <summary>Customers whose first/last name CONTAINS the search value within the organization.</summary>
        Task<IEnumerable<CustomerDto>> GetByNameAsync(Guid organizationId, string name);

        Task<IEnumerable<CustomerDto>> GetAllAsync(Guid organizationId);

        Task<PaginatedResult<CustomerDto>> GetAllPagedAsync(Guid organizationId, int page, int pageSize);
    }
}
