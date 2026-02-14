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
    public class SaleCommandRepository : CommandRepository<Sale>, ISaleCommandRepository
    {
        private readonly AppDbContext _context;
        public SaleCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}