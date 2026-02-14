using Application.Dtos.Expenses;
using Application.QueryRepositories;
using Dapper;
using Infrastructure.Repositories.Queries.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.Repositories.Queries
{
    public class ExpenseQueryRepository : QueryRepository, IExpenseQueryRepository
    {
        public ExpenseQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<ExpenseDto?> GetByIdAsync(Guid id)
        {
            var query = "SELECT Id, BrandId, CategoryId, Amount, ExpenseDate, Notes FROM Expenses WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<ExpenseDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<ExpenseDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, BrandId, CategoryId, Amount, ExpenseDate, Notes FROM Expenses";
            var result = await _connection.QueryAsync<ExpenseDto>(query);
            return result;
        }

        public async Task<IEnumerable<ExpenseDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = "SELECT Id, BrandId, CategoryId, Amount, ExpenseDate, Notes FROM Expenses WHERE BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<ExpenseDto>(query, parameters);
            return result;
        }
    }
}
