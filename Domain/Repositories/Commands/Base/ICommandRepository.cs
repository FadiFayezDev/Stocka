using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repositories.Commands.Base
{
    public interface ICommandRepository<T>
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllTableAsync();
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}