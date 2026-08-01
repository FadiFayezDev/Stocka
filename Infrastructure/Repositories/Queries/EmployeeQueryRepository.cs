using Application.Dtos.Core;
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
    public class EmployeeQueryRepository : QueryRepository, IEmployeeQueryRepository
    {
        public EmployeeQueryRepository(IDbConnection connection, ICurrentUserContext userContext) : base(connection, userContext)
        {
        }

        public async Task<EmployeeDto?> GetByIdAsync(Guid id)
        {
            var query = $@"SELECT id, user_id AS ApplicationUserId, brand_id AS BrandId, branch_id AS BranchId, job_title AS JobTitle, salary AS Salary, hire_date AS HireDate, is_active AS IsActive 
                           FROM {TableEmployees}
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
            var result = await _connection.QuerySingleOrDefaultAsync<EmployeeDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllTableAsync()
        {
            var query = $@"SELECT id, user_id AS ApplicationUserId, brand_id AS BrandId, branch_id AS BranchId, job_title AS JobTitle, salary AS Salary, hire_date AS HireDate, is_active AS IsActive 
                           FROM {TableEmployees}
                           WHERE (@BrandId IS NULL OR brand_id = @BrandId)
                             AND (@ApplyBranchFilter = FALSE OR branch_id = @BranchId)";
            var result = await _connection.QueryAsync<EmployeeDto>(query, new
            {
                BrandId = ActiveBrandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            });
            return result;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = $@"SELECT id, user_id AS ApplicationUserId, brand_id AS BrandId, branch_id AS BranchId, job_title AS JobTitle, salary AS Salary, hire_date AS HireDate, is_active AS IsActive 
                           FROM {TableEmployees}
                           WHERE brand_id = @BrandId
                             AND (@ApplyBranchFilter = FALSE OR branch_id = @BranchId)";
            var parameters = new
            {
                BrandId = brandId,
                ApplyBranchFilter = ApplyBranchScope,
                BranchId = ActiveBranchId
            };
            var result = await _connection.QueryAsync<EmployeeDto>(query, parameters);
            return result;
        }
    }
}
