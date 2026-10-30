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
    /// Read side for Customer rows.
    /// <para>
    /// Person data (names/phone) lives on the <c>Parties</c> supertype row, so every
    /// query joins it. Organization scoping goes through <c>PartyOrganizations</c>;
    /// its PK is (PartyId, Organization), so the join returns exactly one row per member.
    /// </para>
    /// </summary>
    internal sealed class CustomerQueryRepository : QueryRepository, ICustomerQueryRepository
    {
        private const string PersonColumns = "P.FirstName, P.LastName, P.PhoneNumber";

        private const string BaseSelect =
            $"SELECT C.Id, {PersonColumns}, C.LoyaltyPoint " +
            "FROM Customers AS C " +
            "JOIN Parties AS P ON P.PartyId = C.Id";

        private static string OrgScopedWhere() =>
            " JOIN PartyOrganizations AS PO ON PO.PartyId = C.Id " +
            "WHERE PO.Organization = @OrganizationId";

        public CustomerQueryRepository(IIdentityDbConnection connection) : base(connection)
        {
        }

        public async Task<CustomerDto?> GetByIdAsync(PartyId id)
        {
            // Retrieval is by the shared key only — no organization scoping.
            const string sql =
                BaseSelect + " WHERE C.Id = @Id";

            return await Connection.QueryFirstOrDefaultAsync<CustomerDto>(sql, new { Id = id.Value });
        }

        public async Task<CustomerDto?> GetByPhoneNumberAsync(Guid organizationId, string phoneNumber)
        {
            var sql = BaseSelect + OrgScopedWhere() + " AND P.PhoneNumber = @PhoneNumber";
            var parameters = new { OrganizationId = organizationId, PhoneNumber = phoneNumber };
            return await Connection.QueryFirstOrDefaultAsync<CustomerDto>(sql, parameters);
        }

        public async Task<IEnumerable<CustomerDto>> GetByNameAsync(Guid organizationId, string name)
        {
            // Parameterized contains-search; SQL wildcards from caller input are escaped.
            var pattern = $"%{EscapeLike(name)}%";
            var sql = BaseSelect + OrgScopedWhere() + " AND (P.FirstName LIKE @Pattern OR P.LastName LIKE @Pattern)";
            return await Connection.QueryAsync<CustomerDto>(
                sql, new { OrganizationId = organizationId, Pattern = pattern });
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync(Guid organizationId)
            => await Connection.QueryAsync<CustomerDto>(
                BaseSelect + OrgScopedWhere() + " ORDER BY P.FirstName, P.LastName",
                new { OrganizationId = organizationId });

        public async Task<PaginatedResult<CustomerDto>> GetAllPagedAsync(Guid organizationId, int page, int pageSize)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var total = await CountScopedAsync(organizationId);

            var offset = (page - 1) * pageSize;
            var sql = BaseSelect + OrgScopedWhere() +
                      " ORDER BY P.FirstName, P.LastName OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            var items = await Connection.QueryAsync<CustomerDto>(
                sql, new { OrganizationId = organizationId, Offset = offset, PageSize = pageSize });

            return new PaginatedResult<CustomerDto>(items.AsList(), total, page, pageSize);
        }

        private async Task<int> CountScopedAsync(Guid organizationId)
        {
            const string countSql =
                """
                SELECT COUNT(*)
                FROM Customers AS C
                JOIN PartyOrganizations AS PO ON PO.PartyId = C.Id
                WHERE PO.Organization = @OrganizationId
                """;
            var parameters = new { OrganizationId = organizationId };
            return await Connection.ExecuteScalarAsync<int>(countSql, parameters);
        }
    }
}
