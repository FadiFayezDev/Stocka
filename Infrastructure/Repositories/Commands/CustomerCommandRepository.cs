using Domain.Contracts;
using Domain.Entities.Accounting;
using Domain.Entities.Core;
using Domain.Entities.Sales;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.Commands
{
    public class CustomerCommandRepository : CommandRepository<Customer>, ICustomerCommandRepository
    {
        private readonly AppDbContext _context;
        public CustomerCommandRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
