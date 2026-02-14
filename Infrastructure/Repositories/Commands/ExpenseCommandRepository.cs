using Domain.Entities.Expenses;
using Domain.Repositories.Commands;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.Commands
{
    public class ExpenseCommandRepository : CommandRepository<Expense>, IExpenseCommandRepository
    {
        private readonly AppDbContext _context;

        public ExpenseCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
