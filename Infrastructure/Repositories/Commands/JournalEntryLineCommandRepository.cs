using Domain.Entities.Accounting;
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
    public class JournalEntryLineCommandRepository : CommandRepository<JournalEntryLine>, IJournalEntryLineCommandRepository
    {
        private readonly AppDbContext _context;

        public JournalEntryLineCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
