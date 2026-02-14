using Domain.Entities.Accounting;
using Domain.Entities.Purchasing;
using Domain.Repositories.Commands;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.Commands
{
    public class SupplierCommandRepository : CommandRepository<Supplier>, ISupplierCommandRepository
    {
        private readonly AppDbContext _context;
        public SupplierCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
