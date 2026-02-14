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
    public class ExpenseCategoryQueryRepository : QueryRepository, IExpenseCategoryQueryRepository
    {
        public ExpenseCategoryQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<ExpenseCategoryDto?> GetByIdAsync(Guid id)
        {
            var query = "SELECT Id, BrandId, Name FROM ExpenseCategories WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<ExpenseCategoryDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<ExpenseCategoryDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, BrandId, Name FROM ExpenseCategories";
            var result = await _connection.QueryAsync<ExpenseCategoryDto>(query);
            return result;
        }

        public async Task<IEnumerable<ExpenseCategoryDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = "SELECT Id, BrandId, Name FROM ExpenseCategories WHERE BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<ExpenseCategoryDto>(query, parameters);
            return result;
        }
    }
}
