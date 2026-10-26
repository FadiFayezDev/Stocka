namespace BuildingBlocks.Application.Common.Interfaces
{
    public interface IRepository<T, TKey>
    {
        Task<T?> GetByIdAsync(TKey id);

        Task AddAsync(T model);
        Task UpdateAsync(T model);
        Task RemoveAsync(T model);
    }
}