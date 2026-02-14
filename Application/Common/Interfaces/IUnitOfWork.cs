using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default); 
        int SaveChange();

        void BeginTransaction();
        Task BeginTransactionAsync();

        void CommitTransaction();
        Task CommitTransactionAsync();

        void RollbackTransaction();
        Task RollbackTransactionAsync();
    }
}
