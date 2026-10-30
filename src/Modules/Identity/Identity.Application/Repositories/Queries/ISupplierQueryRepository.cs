using BuildingBlocks.Application.Common.Models;
using Identity.Contracts.DTOs;
using Identity.Domain.Parties;

namespace Identity.Application.Repositories.Queries
{
    /// <summary>Read-side (CQRS) queries for Supplier (company) role rows, projected to their contract DTOs.</summary>
    public interface ISupplierQueryRepository
    {
        /// <summary>Single supplier by the shared key (which is the owning Party's id).</summary>
        Task<SupplierDto?> GetByIdAsync(PartyId id);

        /// <summary>Exact phone-number match within the organization (contact person's Party phone).</summary>
        Task<SupplierDto?> GetByPhoneNumberAsync(Guid organizationId, string phoneNumber);

        /// <summary>Suppliers whose company name CONTAINS the search value within the organization.</summary>
        Task<IEnumerable<SupplierDto>> GetByNameAsync(Guid organizationId, string name);

        /// <summary>Exact company-name match, used to prevent duplicate suppliers within an organization.</summary>
        Task<SupplierDto?> GetByExactNameAsync(Guid organizationId, string name);

        Task<IEnumerable<SupplierDto>> GetAllAsync(Guid organizationId);

        Task<PaginatedResult<SupplierDto>> GetAllPagedAsync(Guid organizationId, int page, int pageSize);
    }
}
