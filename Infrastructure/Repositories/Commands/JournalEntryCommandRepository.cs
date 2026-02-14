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
    public class JournalEntryCommandRepository : CommandRepository<JournalEntry>, IJournalEntryCommandRepository
    {
        private readonly AppDbContext _context;

        public JournalEntryCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JournalEntry>> GetAllByBrandIdAsync(Guid brandId)
        {
            return _context.JournalEntries
                .Where(a => a.BrandId == brandId)
                .AsNoTracking()
                .AsQueryable();
        }
    }
}
