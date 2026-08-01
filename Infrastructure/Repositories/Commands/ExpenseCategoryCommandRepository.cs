using Domain.Entities.Accounting;
using Domain.Entities.Expenses;
using Domain.Primitives;
using Domain.Repositories.Commands;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.Commands
{
    public class ExpenseCategoryCommandRepository : CommandRepository<ExpenseCategory, ExpenseCategoryId>, IExpenseCategoryCommandRepository
    {
        private readonly AppDbContext _context;

        public ExpenseCategoryCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
