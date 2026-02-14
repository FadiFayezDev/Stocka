using Domain.Entities.Accounting;
using Domain.Entities.Core;
using Domain.Entities.Products;
using Domain.Repositories.Commands;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.Commands
{
    public class BrandCommandRepository : CommandRepository<Brand>, IBrandCommandRepository
    {
        private readonly AppDbContext _context;
        public BrandCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
