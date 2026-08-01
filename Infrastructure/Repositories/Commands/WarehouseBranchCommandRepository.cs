using Domain.Entities.Products;
using Domain.Primitives;
using Domain.Repositories.Commands;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.Commands
{
    public class WarehouseBranchCommandRepository : CommandRepository<WarehouseBranch, WarehouseBranchId>, IWarehouseBranchCommandRepository
    {
        private readonly AppDbContext _context;
        public WarehouseBranchCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}