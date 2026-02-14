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
    public class PurchaseItemCommandRepository : CommandRepository<PurchaseItem>, IPurchaseItemCommandRepository
    {
        private readonly AppDbContext _context;
        public PurchaseItemCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
