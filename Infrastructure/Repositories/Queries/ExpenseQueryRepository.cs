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
            var query = $"SELECT id, brand_id AS BrandId, category_id AS CategoryId, amount AS Amount, expense_date AS ExpenseDate, notes AS Notes FROM {TableExpenses} WHERE id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<ExpenseDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<ExpenseDto>> GetAllTableAsync()
        {
            var query = $"SELECT id, brand_id AS BrandId, category_id AS CategoryId, amount AS Amount, expense_date AS ExpenseDate, notes AS Notes FROM {TableExpenses}";
            var result = await _connection.QueryAsync<ExpenseDto>(query);
            return result;
        }

        public async Task<IEnumerable<ExpenseDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $"SELECT id, brand_id AS BrandId, category_id AS CategoryId, amount AS Amount, expense_date AS ExpenseDate, notes AS Notes FROM {TableExpenses} WHERE brand_id = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<ExpenseDto>(query, parameters);
            return result;
        }
    }
}
