using Domain.Contracts;
using Domain.Entities.Accounting;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.Commands
{
    public class AccountCommandRepository : CommandRepository<Account>, IAccountCommandRepository
    {
        private readonly AppDbContext _context;
        public AccountCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}