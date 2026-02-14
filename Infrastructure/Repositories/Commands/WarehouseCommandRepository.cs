using Domain.Entities.Accounting;
using Domain.Entities.Products;
using Domain.Repositories.Commands;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.Commands
{
    public class WarehouseCommandRepository : CommandRepository<Warehouse>, IWarehouseCommandRepository
    {
        private readonly AppDbContext _context;
        public WarehouseCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}