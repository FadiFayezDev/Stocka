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
    public class ProductCommandRepository : CommandRepository<Product>, IProductCommandRepository
    {
        private readonly AppDbContext _context;
        public ProductCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
