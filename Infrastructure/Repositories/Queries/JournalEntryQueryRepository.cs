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
            var query = "SELECT Id, BrandId, Description, EntryDate FROM JournalEntries WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<JournalEntryDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<JournalEntryDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, BrandId, Description, EntryDate FROM JournalEntries";
            var result = await _connection.QueryAsync<JournalEntryDto>(query);
            return result;
        }

        public async Task<IEnumerable<JournalEntryDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = "SELECT Id, BrandId, Description, EntryDate FROM JournalEntries WHERE BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<JournalEntryDto>(query, parameters);
            return result;
        }
    }
}
