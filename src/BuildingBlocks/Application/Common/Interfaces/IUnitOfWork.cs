namespace BuildingBlocks.Application.Common.Interfaces
{
    /// <summary>
    /// Shared unit-of-work contract — the single source of truth.
    /// Each module exposes its own module-scoped interface that inherits this one
    /// (e.g. <c>ICatalogUnitOfWork</c>, <c>IInventoryUnitOfWork</c>, <c>ICoreUnitOfWork</c>)
    /// so DI registrations never collide across modules while the contract itself is defined once.
    /// </summary>
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();

        void BeginTransaction();
        Task BeginTransactionAsync();

        void CommitTransaction();
        Task CommitTransactionAsync();

        void RollbackTransaction();
        Task RollbackTransactionAsync();
    }
}
