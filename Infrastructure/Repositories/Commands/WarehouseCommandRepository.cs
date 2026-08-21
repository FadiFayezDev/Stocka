using Domain.Entities.Accounting;
using Domain.Entities.Products;
using Domain.Primitives;
using Domain.Repositories.Commands;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.Commands
{
    public class WarehouseCommandRepository : CommandRepository<Warehouse, WarehouseId>, IWarehouseCommandRepository
    {
        private readonly AppDbContext _context;
        public WarehouseCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Warehouse?> GetByIdAsync(Guid id)
        {
            return await _context.Set<Warehouse>()
                .Include(w => w.WarehouseBatches)
                .FirstOrDefaultAsync(w => w.Id == new WarehouseId(id));
        }

        public async Task<IEnumerable<Guid>> GetLinkedBranchIdsAsync(Guid warehouseId)
        {
            return await _context.WarehouseBranches
                .Where(wb => wb.WarehouseId == new WarehouseId(warehouseId))
                .Select(wb => wb.BranchId.Value)
                .ToListAsync();
        }

        public async Task ReplaceBranchLinksAsync(Guid warehouseId, IEnumerable<Guid> branchIds, Guid brandId)
        {
            var whId = new WarehouseId(warehouseId);
            var brand = new BrandId(brandId);

            var existing = await _context.WarehouseBranches
                .Where(wb => wb.WarehouseId == whId)
                .ToListAsync();

            var requested = branchIds.Distinct().Select(b => new BranchId(b)).ToList();

            var toRemove = existing
                .Where(wb => !requested.Contains(wb.BranchId))
                .ToList();

            var toAdd = requested
                .Where(b => !existing.Any(wb => wb.BranchId == b))
                .ToList();

            foreach (var link in toRemove)
                _context.WarehouseBranches.Remove(link);

            foreach (var branchId in toAdd)
                _context.WarehouseBranches.Add(new WarehouseBranch(brand, branchId, whId));
        }
    }
}