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
            var query = "SELECT Id, JournalEntryId, AccountId, DebitAmount, CreditAmount FROM JournalEntryLines WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<JournalEntryLineDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<JournalEntryLineDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, JournalEntryId, AccountId, DebitAmount, CreditAmount FROM JournalEntryLines";
            var result = await _connection.QueryAsync<JournalEntryLineDto>(query);
            return result;
        }

        public async Task<IEnumerable<JournalEntryLineDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = @"SELECT jel.Id, jel.JournalEntryId, jel.AccountId, jel.DebitAmount, jel.CreditAmount 
                         FROM JournalEntryLines jel
                         INNER JOIN JournalEntries je ON jel.JournalEntryId = je.Id
                         WHERE je.BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<JournalEntryLineDto>(query, parameters);
            return result;
        }
    }
}
