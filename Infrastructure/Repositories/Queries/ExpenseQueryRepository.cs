using Application.Dtos.Expenses;
using Application.Common.Interfaces;
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
        public ExpenseQueryRepository(IDbConnection connection, ICurrentUserContext userContext) : base(connection, userContext)
        {
        }

        public async Task<ExpenseDto?> GetByIdAsync(Guid id)
        {
            var query = $@"SELECT id, brand_id AS BrandId, branch_id AS BranchId, category_id AS CategoryId, amount AS Amount, expense_date AS ExpenseDate, notes AS Notes 
                           FROM {TableExpenses}
                           WHERE id = @Id
                             AND (@BrandId IS NULL OR brand_id = @BrandId)
                             AND (@ApplyBranchFilter = FALSE OR branch_id = @BranchId)";
            var parameters = new
            {
                Id = id,
                BrandId = ActiveBrandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            };
            var result = await _connection.QuerySingleOrDefaultAsync<ExpenseDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<ExpenseDto>> GetAllTableAsync()
        {
            var query = $@"SELECT id, brand_id AS BrandId, branch_id AS BranchId, category_id AS CategoryId, amount AS Amount, expense_date AS ExpenseDate, notes AS Notes 
                           FROM {TableExpenses}
                           WHERE (@BrandId IS NULL OR brand_id = @BrandId)
                             AND (@ApplyBranchFilter = FALSE OR branch_id = @BranchId)";
            var result = await _connection.QueryAsync<ExpenseDto>(query, new
            {
                BrandId = ActiveBrandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            });
            return result;
        }

        public async Task<IEnumerable<ExpenseDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $@"SELECT id, brand_id AS BrandId, branch_id AS BranchId, category_id AS CategoryId, amount AS Amount, expense_date AS ExpenseDate, notes AS Notes 
                           FROM {TableExpenses}
                           WHERE brand_id = @BrandId
                             AND (@ApplyBranchFilter = FALSE OR branch_id = @BranchId)";
            var parameters = new
            {
                BrandId = brandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            };
            var result = await _connection.QueryAsync<ExpenseDto>(query, parameters);
            return result;
        }
    }
}
