using Domain.Bases;
using Domain.Repositories.Commands.Base;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Base
{
    public class CommandRepository<T, TKey> : ICommandRepository<T>
        where T : Entity<TKey>
    {
        private readonly AppDbContext _context;

        public CommandRepository(AppDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(KeyFromGuid(id));
        }

        public virtual async Task<IEnumerable<T>> GetAllTableAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<bool> CreateAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            return true;
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return true;
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            var entry = _context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                _context.Attach(entity);
                entry.State = EntityState.Modified;
            }

            return true;
        }

        private static TKey KeyFromGuid(Guid id)
            => (TKey)Activator.CreateInstance(typeof(TKey), id)!;
    }
}
