using Domain.Entities.Accounting;
using Domain.Entities.Sales;
using Domain.Repositories.Commands;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.Commands
{
    public class SaleItemCommandRepository : CommandRepository<SaleItem>, ISaleItemCommandRepository
    {
        private readonly AppDbContext _context;
        public SaleItemCommandRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
