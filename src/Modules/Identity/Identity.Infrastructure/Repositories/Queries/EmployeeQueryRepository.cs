using BuildingBlocks.Application.Common.Models;
using Dapper;
using Identity.Application.Common.Interfaces;
using Identity.Application.Repositories.Queries;
using Identity.Contracts.DTOs;
using Identity.Domain.Parties;
using Identity.Infrastructure.Repositories.Queries.Bases;
using System.Data;

namespace Identity.Infrastructure.Repositories.Queries
{
    /// <summary>
    /// Read side for Employee rows. Same membership-scoping shape as
    /// <see cref="CustomerQueryRepository"/> — person data comes from the
    /// <c>Parties</c> supertype row; organization membership from <c>PartyOrganizations</c>.
    /// </summary>
    internal sealed class EmployeeQueryRepository : QueryRepository, IEmployeeQueryRepository
    {
        private const string PersonColumns = "P.FirstName, P.LastName, P.PhoneNumber";

        private const string BaseSelect =
            $"SELECT E.PartyId AS Id, E.JobTitle, {PersonColumns} " +
            "FROM Employees AS E " +
            "JOIN Parties AS P ON P.PartyId = E.PartyId";

        private static string OrgScopedWhere() =>
            " JOIN PartyOrganizations AS PO ON PO.PartyId = E.PartyId " +
            "WHERE PO.Organization = @OrganizationId";

        private const string CountSql =
            """
            SELECT COUNT(*)
            FROM Employees AS E
            JOIN PartyOrganizations AS PO ON PO.PartyId = E.PartyId
            WHERE PO.Organization = @OrganizationId
            """;

        public EmployeeQueryRepository(IIdentityDbConnection connection) : base(connection)
        {
        }

        public async Task<EmployeeDto?> GetByIdAsync(PartyId id)
        {
            // Retrieval is by the shared key only — no organization scoping.
            const string sql = BaseSelect + " WHERE E.PartyId = @Id";
            return await Connection.QueryFirstOrDefaultAsync<EmployeeDto>(sql, new { Id = id.Value });
        }

        public async Task<EmployeeDto?> GetByPhoneNumberAsync(Guid organizationId, string phoneNumber)
        {
            var sql = BaseSelect + OrgScopedWhere() + " AND P.PhoneNumber = @PhoneNumber";
            return await Connection.QueryFirstOrDefaultAsync<EmployeeDto>(
                sql, new { OrganizationId = organizationId, PhoneNumber = phoneNumber });
        }

        public async Task<IEnumerable<EmployeeDto>> GetByNameAsync(Guid organizationId, string name)
        {
            // Parameterized contains-search; SQL wildcards from caller input are escaped.
            var pattern = $"%{EscapeLike(name)}%";
            var sql = BaseSelect + OrgScopedWhere() + " AND (P.FirstName LIKE @Pattern OR P.LastName LIKE @Pattern)";
            return await Connection.QueryAsync<EmployeeDto>(
                sql, new { OrganizationId = organizationId, Pattern = pattern });
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync(Guid organizationId)
            => await Connection.QueryAsync<EmployeeDto>(
                BaseSelect + OrgScopedWhere() + " ORDER BY P.FirstName, P.LastName",
                new { OrganizationId = organizationId });

        public async Task<PaginatedResult<EmployeeDto>> GetAllPagedAsync(Guid organizationId, int page, int pageSize)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var total = await Connection.ExecuteScalarAsync<int>(CountSql, new { OrganizationId = organizationId });

            var offset = (page - 1) * pageSize;
            var sql = BaseSelect + OrgScopedWhere() +
                      " ORDER BY P.FirstName, P.LastName OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            var items = await Connection.QueryAsync<EmployeeDto>(
                sql, new { OrganizationId = organizationId, Offset = offset, PageSize = pageSize });

            return new PaginatedResult<EmployeeDto>(items.AsList(), total, page, pageSize);
        }
    }
}
