using Catalog.Application.Common.Interfaces;
using Catalog.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace Catalog.Infrastructure.Services
{
    internal class UnitOfWork : ICatalogUnitOfWork, IDisposable
    {
        private readonly CatalogDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(CatalogDbContext context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void BeginTransaction()
        {
            if (_transaction != null)
                return;

            _transaction = _context.Database.BeginTransaction();
        }

        public async Task BeginTransactionAsync(
            CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
                return;

            _transaction = await _context.Database
                .BeginTransactionAsync(cancellationToken);
        }

        public void CommitTransaction()
        {
            try
            {
                _transaction?.Commit();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public async Task CommitTransactionAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (_transaction != null)
                    await _transaction.CommitAsync(cancellationToken);
            }
            finally
            {
                if (_transaction != null)
                    await _transaction.DisposeAsync();

                _transaction = null;
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (_transaction != null)
                    await _transaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                if (_transaction != null)
                    await _transaction.DisposeAsync();

                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            // Do not dispose _context — it is scoped and owned by DI container
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
