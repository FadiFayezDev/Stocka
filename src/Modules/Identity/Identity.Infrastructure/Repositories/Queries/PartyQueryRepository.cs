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
    /// Read side over the <c>Parties</c> supertype table (the shared person table).
    /// Membership rows per party are batch-loaded and attached, so a party travels
    /// with its <c>OrganizationIds</c> exactly like the aggregate would.
    /// </summary>
    internal sealed class PartyQueryRepository : QueryRepository, IPartyQueryRepository
    {
        private const string BaseSelect =
            "SELECT P.PartyId AS Id, P.FirstName, P.LastName, P.PhoneNumber FROM Parties AS P";

        private const string OrgScopedBaseSelect =
            "SELECT P.PartyId AS Id, P.FirstName, P.LastName, P.PhoneNumber " +
            "FROM Parties AS P " +
            "JOIN PartyOrganizations AS PO ON PO.PartyId = P.PartyId " +
            "WHERE PO.Organization = @OrganizationId";

        private const string OrgScopedCountSql =
            """
            SELECT COUNT(*)
            FROM Parties AS P
            JOIN PartyOrganizations AS PO ON PO.PartyId = P.PartyId
            WHERE PO.Organization = @OrganizationId
            """;

        public PartyQueryRepository(IIdentityDbConnection connection) : base(connection)
        {
        }

        public async Task<PartyDto?> GetByIdAsync(PartyId id)
        {
            const string sql = BaseSelect + " WHERE P.PartyId = @Id";
            var row = await Connection.QueryFirstOrDefaultAsync<PartyRow>(sql, new { Id = id.Value });
            return row is null ? null : await AttachOrganizationsAsync(row);
        }

        public async Task<PartyDto?> GetByPhoneNumberAsync(Guid organizationId, string phoneNumber)
        {
            var sql = OrgScopedBaseSelect + " AND P.PhoneNumber = @PhoneNumber";
            var row = await Connection.QueryFirstOrDefaultAsync<PartyRow>(
                sql, new { OrganizationId = organizationId, PhoneNumber = phoneNumber });
            return row is null ? null : await AttachOrganizationsAsync(row);
        }

        public async Task<IEnumerable<PartyDto>> GetAllAsync(Guid organizationId)
        {
            var rows = await Connection.QueryAsync<PartyRow>(
                OrgScopedBaseSelect + " ORDER BY P.FirstName, P.LastName", new { OrganizationId = organizationId });
            return await AttachOrganizationsAsync(rows.AsList());
        }

        public async Task<PaginatedResult<PartyDto>> GetPagedAsync(Guid organizationId, int page, int pageSize)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var total = await Connection.ExecuteScalarAsync<int>(
                OrgScopedCountSql, new { OrganizationId = organizationId });

            var offset = (page - 1) * pageSize;
            var sql = OrgScopedBaseSelect +
                      " ORDER BY P.FirstName, P.LastName OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            var rows = await Connection.QueryAsync<PartyRow>(
                sql, new { OrganizationId = organizationId, Offset = offset, PageSize = pageSize });

            return new PaginatedResult<PartyDto>(
                await AttachOrganizationsAsync(rows.AsList()), total, page, pageSize);
        }

        /// <summary>Flat projection target — the DTO is assembled once memberships are attached.</summary>
        private sealed record PartyRow(Guid Id, string FirstName, string LastName, string PhoneNumber);

        /// <summary>Single-party attach: one extra query for its organization memberships.</summary>
        private async Task<PartyDto> AttachOrganizationsAsync(PartyRow row)
        {
            var organizationIds = (await Connection.QueryAsync<Guid>(
                "SELECT Organization FROM PartyOrganizations WHERE PartyId = @PartyId",
                new { PartyId = row.Id })).AsList();

            return new PartyDto(row.Id, row.FirstName, row.LastName, row.PhoneNumber, organizationIds);
        }

        /// <summary>Batch attach for lists/pages: ONE extra query total, grouped in memory.</summary>
        private async Task<IReadOnlyList<PartyDto>> AttachOrganizationsAsync(IReadOnlyList<PartyRow> rows)
        {
            if (rows.Count == 0)
                return [];

            var partyIds = rows.Select(r => r.Id).ToArray();
            var memberships = await Connection.QueryAsync<(Guid PartyId, Guid Organization)>(
                "SELECT PartyId, Organization FROM PartyOrganizations WHERE PartyId IN @PartyIds",
                new { PartyIds = partyIds });

            var organizationsByParty = memberships
                .GroupBy(m => m.PartyId)
                .ToDictionary(
                    g => g.Key,
                    g => (IReadOnlyList<Guid>)g.Select(m => m.Organization).ToList());

            return rows
                .Select(r => new PartyDto(
                    r.Id,
                    r.FirstName,
                    r.LastName,
                    r.PhoneNumber,
                    organizationsByParty.GetValueOrDefault(r.Id, [])))
                .ToList();
        }
    }
}
