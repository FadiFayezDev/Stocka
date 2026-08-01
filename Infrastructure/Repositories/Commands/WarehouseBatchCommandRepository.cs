using Domain.Entities.Products;
using Domain.Primitives;
using Domain.Repositories.Commands;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure.Repositories.Commands
{
    public class WarehouseBatchCommandRepository : CommandRepository<WarehouseBatch, WarehouseBatchId>, IWarehouseBatchCommandRepository
    {
        private readonly AppDbContext _context;
        public WarehouseBatchCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<WarehouseBatch?> GetByWarehouseAndBatchAsync(Guid warehouseId, Guid batchId)
        {
            return await _context.WarehouseBatches
                .FirstOrDefaultAsync(wb => wb.WarehouseId.Value == warehouseId && wb.BatchId.Value == batchId);
        }

        public async Task<IReadOnlyList<WarehouseBatch>> GetByBatchAsync(Guid batchId)
        {
            return await _context.WarehouseBatches
                .Where(wb => wb.BatchId.Value == batchId)
                .ToListAsync();
        }
    }
}