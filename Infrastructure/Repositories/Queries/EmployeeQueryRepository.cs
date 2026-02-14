using Application.Dtos.Core;
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
        public EmployeeQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<EmployeeDto?> GetByIdAsync(Guid id)
        {
            var query = "SELECT Id, UserId, BrandId, JobTitle, Salary, HireDate, IsActive FROM Employees WHERE Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<EmployeeDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllTableAsync()
        {
            var query = "SELECT Id, UserId, BrandId, JobTitle, Salary, HireDate, IsActive FROM Employees";
            var result = await _connection.QueryAsync<EmployeeDto>(query);
            return result;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = "SELECT Id, UserId, BrandId, JobTitle, Salary, HireDate, IsActive FROM Employees WHERE BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<EmployeeDto>(query, parameters);
            return result;
        }
    }
}
