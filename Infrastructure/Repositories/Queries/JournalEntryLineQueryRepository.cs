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
    public class JournalEntryLineQueryRepository : QueryRepository, IJournalEntryLineQueryRepository
    {
        public JournalEntryLineQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<JournalEntryLineDto?> GetByIdAsync(Guid id)
        {
            var query = $"SELECT id, journal_entry_id AS JournalEntryId, account_id AS AccountId, debit AS DebitAmount, credit AS CreditAmount FROM {TableJournalEntryLines} WHERE id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<JournalEntryLineDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<JournalEntryLineDto>> GetAllTableAsync()
        {
            var query = $"SELECT id, journal_entry_id AS JournalEntryId, account_id AS AccountId, debit AS DebitAmount, credit AS CreditAmount FROM {TableJournalEntryLines}";
            var result = await _connection.QueryAsync<JournalEntryLineDto>(query);
            return result;
        }

        public async Task<IEnumerable<JournalEntryLineDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $@"SELECT jel.id, jel.journal_entry_id AS JournalEntryId, jel.account_id AS AccountId, jel.debit AS DebitAmount, jel.credit AS CreditAmount 
                         FROM {TableJournalEntryLines} jel
                         INNER JOIN {TableJournalEntries} je ON jel.journal_entry_id = je.id
                         WHERE je.brand_id = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<JournalEntryLineDto>(query, parameters);
            return result;
        }
    }
}
