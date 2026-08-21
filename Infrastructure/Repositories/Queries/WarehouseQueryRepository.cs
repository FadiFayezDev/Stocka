using Application.Dtos;
using Application.Dtos.Products;
using Application.QueryRepositories;
using Dapper;
using Infrastructure.Repositories.Queries.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.Repositories.Queries
{
    public class WarehouseQueryRepository : QueryRepository, IWarehouseQueryRepository
    {
        public WarehouseQueryRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<WarehouseDto?> GetByIdAsync(Guid id)
        {
            var query = $"SELECT id, name AS Name, type AS Type, is_active AS IsActive FROM Warehouses WHERE id = @Id";
            var parameters = new { Id = id };
            var result = await _connection.QuerySingleOrDefaultAsync<WarehouseDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<WarehouseDto>> GetAllTableAsync()
        {
            var query = $"SELECT id, name AS Name, type AS Type, is_active AS IsActive FROM Warehouses";
            var result = await _connection.QueryAsync<WarehouseDto>(query);
            return result;
        }

        public async Task<IEnumerable<WarehouseDto>> GetAllByBrandIdAsync(Guid brandId)
        {
            var query = """
                SELECT 
                    w.id, 
                    w.name AS Name, 
                    w.type AS Type,
                    w.location AS Location, 
                    w.description AS Description,
                    w.is_active AS IsActive 
                FROM Warehouses w
                WHERE w.brand_id = @BrandId
                """;
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<WarehouseDto>(query, parameters);
            return result;
        }

        public async Task<IEnumerable<WarehouseDto>> GetByBranchIdAsync(Guid branchId)
        {
            var query = """
                SELECT 
                    w.id, 
                    w.name AS Name, 
                    w.type AS Type,
                    w.location AS Location, 
                    w.description AS Description,
                    w.is_active AS IsActive 
                FROM Warehouses w
                INNER JOIN warehouse_branch wb ON wb.warehouse_id = w.id
                WHERE wb.branch_id = @BranchId
                """;
            var parameters = new { BranchId = branchId };
            var result = await _connection.QueryAsync<WarehouseDto>(query, parameters);
            return result;
        }

        public async Task<IDictionary<Guid, List<string>>> GetBranchNamesByWarehouseIdsAsync(Guid brandId)
        {
            var query = """
                SELECT 
                    wb.warehouse_id AS WarehouseId, 
                    b.name AS BranchName
                FROM warehouse_branch wb
                INNER JOIN branches b ON b.id = wb.branch_id
                WHERE wb.brand_id = @BrandId AND b.id <> '00000000-0000-0000-0000-000000000000'
                ORDER BY b.name
                """;
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<WarehouseBranchNameRow>(query, parameters);

            var dictionary = new Dictionary<Guid, List<string>>();
            foreach (var row in result)
            {
                if (!dictionary.TryGetValue(row.WarehouseId, out var list))
                {
                    list = new List<string>();
                    dictionary[row.WarehouseId] = list;
                }
                list.Add(row.BranchName);
            }

            return dictionary;
        }

        public async Task<IDictionary<Guid, List<string>>> GetWarehouseNamesByBranchIdsAsync(Guid brandId)
        {
            var query = """
                SELECT 
                    wb.branch_id AS BranchId, 
                    w.name AS WarehouseName
                FROM warehouse_branch wb
                INNER JOIN warehouses w ON w.id = wb.warehouse_id
                WHERE wb.brand_id = @BrandId AND wb.branch_id <> '00000000-0000-0000-0000-000000000000'
                ORDER BY w.name
                """;
            var parameters = new { BrandId = brandId };
            var result = await _connection.QueryAsync<BranchWarehouseNameRow>(query, parameters);

            var dictionary = new Dictionary<Guid, List<string>>();
            foreach (var row in result)
            {
                if (!dictionary.TryGetValue(row.BranchId, out var list))
                {
                    list = new List<string>();
                    dictionary[row.BranchId] = list;
                }
                list.Add(row.WarehouseName);
            }

            return dictionary;
        }

        private class WarehouseBranchNameRow
        {
            public Guid WarehouseId { get; set; }
            public string BranchName { get; set; } = null!;
        }

        private class BranchWarehouseNameRow
        {
            public Guid BranchId { get; set; }
            public string WarehouseName { get; set; } = null!;
        }
    }
}