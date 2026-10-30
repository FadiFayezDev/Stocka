using BuildingBlocks.Application.Common.Models;
using Identity.Contracts.DTOs;
using Identity.Domain.Parties;

namespace Identity.Application.Repositories.Queries
{
    /// <summary>Read-side (CQRS) queries for Employee role rows, projected to their contract DTOs.</summary>
    public interface IEmployeeQueryRepository
    {
        /// <summary>Single employee by the shared key (which is the owning Party's id).</summary>
        Task<EmployeeDto?> GetByIdAsync(PartyId id);

        /// <summary>Exact phone-number match within the organization (Party phone).</summary>
        Task<EmployeeDto?> GetByPhoneNumberAsync(Guid organizationId, string phoneNumber);

        /// <summary>Employees whose first/last name CONTAINS the search value within the organization.</summary>
        Task<IEnumerable<EmployeeDto>> GetByNameAsync(Guid organizationId, string name);

        Task<IEnumerable<EmployeeDto>> GetAllAsync(Guid organizationId);

        Task<PaginatedResult<EmployeeDto>> GetAllPagedAsync(Guid organizationId, int page, int pageSize);
    }
}
