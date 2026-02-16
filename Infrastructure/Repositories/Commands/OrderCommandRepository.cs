using Domain.Entities.Accounting;
using Domain.Entities.Orders;
using Domain.Repositories.Commands;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.Commands
{
    public class OrderCommandRepository : CommandRepository<Order>, IOrderCommandRepository
    {
        private readonly AppDbContext _context;
        public OrderCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}