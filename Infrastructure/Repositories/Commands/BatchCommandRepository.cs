using Domain.Entities.Products;
using Domain.Primitives;
using Domain.Repositories.Commands;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure.Repositories.Commands
{
    public class BatchCommandRepository : CommandRepository<Batch, BatchId>, IBatchCommandRepository
    {
        private readonly AppDbContext _context;
        public BatchCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Batch>> GetAvailableBatchesForProductAsync(Guid productId, Guid brandId)
        {
            return await _context.Batches
                .Where(b => b.ProductId == new ProductId(productId)
                         && b.BrandId == new BrandId(brandId)
                         && b.RemainingQuantity > 0)
                .OrderBy(b => b.CreatedAt)
                .ToListAsync();
        }
    }
}
