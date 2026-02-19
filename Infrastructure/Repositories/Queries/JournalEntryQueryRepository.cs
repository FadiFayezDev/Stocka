using Application.Dtos.Accounting;
using Application.QueryRepositories;
using Dapper;
using Infrastructure.Repositories.Queries.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.Repositories.Queries
{
    public class JournalEntryQueryRepository : QueryRepository, IJournalEntryQueryRepository
    {
        public JournalEntryQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<JournalEntryDto?> GetByIdAsync(Guid id)
        {
            var query = $"SELECT id, brand_id AS BrandId, description AS Description, entry_date AS EntryDate FROM {TableJournalEntries} WHERE id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<JournalEntryDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<JournalEntryDto>> GetAllTableAsync()
        {
            var query = $"SELECT id, brand_id AS BrandId, description AS Description, entry_date AS EntryDate FROM {TableJournalEntries}";
            var result = await _connection.QueryAsync<JournalEntryDto>(query);
            return result;
        }

        public async Task<IEnumerable<JournalEntryDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $"SELECT id, brand_id AS BrandId, description AS Description, entry_date AS EntryDate FROM {TableJournalEntries} WHERE brand_id = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<JournalEntryDto>(query, parameters);
            return result;
        }
    }
}
