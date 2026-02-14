using Domain.Bases;
using Domain.Repositories.Commands.Base;
using Infrastructure.Contexts;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Repositories.Base
{
    public class CommandRepository<T> : ICommandRepository<T> where T : class, IEntity<Guid>
    {
        private AppDbContext _context;
        public CommandRepository(AppDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllTableAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<bool> CreateAsync(T entity)
        {
            entity.SetKey(Guid.NewGuid());
            var createdEntity = _context.Set<T>().Add(entity);
            return true;
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
           _context.Set<T>().Remove(entity);
            return true;
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            if(entity.GetKey() is Guid id)
            {
                var existingEntity = await _context.Set<T>().FindAsync(id);
                if (existingEntity == null)
                    return false;
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                return true;
            }
            return false;
        }
    }
}