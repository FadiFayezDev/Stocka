using Application.Dtos.Auth;
using Application.QueryRepositories;
using Dapper;
using Infrastructure.Repositories.Queries.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.Repositories.Queries
{
    public class UserQueryRepository : QueryRepository, IUserQueryRepository
    {
        public UserQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<UserSummaryDto?> GetByIdAsync(Guid id)
        {
            var query = @"SELECT 
                         u.Id, 
                         u.FirstName, 
                         u.LastName, 
                         u.UserName, 
                         u.Email 
                         FROM AspNetUsers u WHERE u.Id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<UserSummaryDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<UserSummaryDto>> GetAllTableAsync()
        {
            var query = @"SELECT 
                         u.Id, 
                         u.FirstName, 
                         u.LastName, 
                         u.UserName, 
                         u.Email 
                         FROM AspNetUsers u";
            var result = await _connection.QueryAsync<UserSummaryDto>(query);
            return result;
        }

        public async Task<IEnumerable<UserSummaryDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = @"SELECT DISTINCT 
                         u.Id, 
                         u.FirstName, 
                         u.LastName, 
                         u.UserName, 
                         u.Email 
                         FROM AspNetUsers u
                         INNER JOIN BrandUsers bu ON u.Id = bu.UserId
                         WHERE bu.BrandId = @BrandId";
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<UserSummaryDto>(query, parameters);
            return result;
        }
    }
}
