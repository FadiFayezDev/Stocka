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
    public class AccountQueryRepository : QueryRepository, IAccountQueryRepository
    {
        public AccountQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<AccountDto?> GetByIdAsync(Guid id)
        {
            var query = "SELECT Id, BrandId, Name, Type FROM Accounts WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<AccountDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<AccountDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, BrandId, Name, Type FROM Accounts";
            var result = await _connection.QueryAsync<AccountDto>(query);
            return result;
        }

        public async Task<IEnumerable<AccountDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = "SELECT Id, BrandId, Name, Type FROM Accounts WHERE BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<AccountDto>(query, parameters);
            return result;
        }
    }
}