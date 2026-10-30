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
    /// Read side for Supplier (company) rows. Person data of the contact person is joined
    /// from the <c>Parties</c> supertype row; the company data (Name/Email/Address) is
    /// supplier-specific. Organization scoping goes through <c>PartyOrganizations</c>.
    /// </summary>
    internal sealed class SupplierQueryRepository : QueryRepository, ISupplierQueryRepository
    {
        private const string PersonColumns = "P.FirstName, P.LastName, P.PhoneNumber";

        private const string BaseSelect =
            $"SELECT S.PartyId AS Id, S.Name, S.Email, S.Address, {PersonColumns} " +
            "FROM Suppliers AS S " +
            "JOIN Parties AS P ON P.PartyId = S.PartyId";

        private static string OrgScopedWhere() =>
            " JOIN PartyOrganizations AS PO ON PO.PartyId = S.PartyId " +
            "WHERE PO.Organization = @OrganizationId";

        private const string CountSql =
            """
            SELECT COUNT(*)
            FROM Suppliers AS S
            JOIN PartyOrganizations AS PO ON PO.PartyId = S.PartyId
            WHERE PO.Organization = @OrganizationId
            """;

        public SupplierQueryRepository(IIdentityDbConnection connection) : base(connection)
        {
        }

        public async Task<SupplierDto?> GetByIdAsync(PartyId id)
        {
            // Retrieval is by the shared key only — no organization scoping.
            const string sql = BaseSelect + " WHERE S.PartyId = @Id";
            return await Connection.QueryFirstOrDefaultAsync<SupplierDto>(sql, new { Id = id.Value });
        }

        public async Task<SupplierDto?> GetByPhoneNumberAsync(Guid organizationId, string phoneNumber)
        {
            var sql = BaseSelect + OrgScopedWhere() + " AND P.PhoneNumber = @PhoneNumber";
            return await Connection.QueryFirstOrDefaultAsync<SupplierDto>(
                sql, new { OrganizationId = organizationId, PhoneNumber = phoneNumber });
        }

        public async Task<IEnumerable<SupplierDto>> GetByNameAsync(Guid organizationId, string name)
        {
            // Parameterized contains-search on the company (trade) name; wildcards escaped.
            var pattern = $"%{EscapeLike(name)}%";
            var sql = BaseSelect + OrgScopedWhere() + " AND S.Name LIKE @Pattern";
            return await Connection.QueryAsync<SupplierDto>(
                sql, new { OrganizationId = organizationId, Pattern = pattern });
        }

        public async Task<SupplierDto?> GetByExactNameAsync(Guid organizationId, string name)
        {
            var sql = BaseSelect + OrgScopedWhere() + " AND S.Name = @Name";
            return await Connection.QueryFirstOrDefaultAsync<SupplierDto>(
                sql, new { OrganizationId = organizationId, Name = name });
        }

        public async Task<IEnumerable<SupplierDto>> GetAllAsync(Guid organizationId)
            => await Connection.QueryAsync<SupplierDto>(
                BaseSelect + OrgScopedWhere() + " ORDER BY S.Name",
                new { OrganizationId = organizationId });

        public async Task<PaginatedResult<SupplierDto>> GetAllPagedAsync(Guid organizationId, int page, int pageSize)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var total = await Connection.ExecuteScalarAsync<int>(CountSql, new { OrganizationId = organizationId });

            var offset = (page - 1) * pageSize;
            var sql = BaseSelect + OrgScopedWhere() +
                      " ORDER BY S.Name OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            var items = await Connection.QueryAsync<SupplierDto>(
                sql, new { OrganizationId = organizationId, Offset = offset, PageSize = pageSize });

            return new PaginatedResult<SupplierDto>(items.AsList(), total, page, pageSize);
        }
    }
}
