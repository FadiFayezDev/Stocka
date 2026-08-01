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
    public class StockMovementCommandRepository : CommandRepository<StockMovement, StockMovementId>, IStockMovementCommandRepository
    {
        private readonly AppDbContext _context;
        public StockMovementCommandRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
