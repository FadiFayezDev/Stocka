using BuildingBlocks.Application.Common.Interfaces;
using BuildingBlocks.Domain.Bases;
using Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories.Commands.Bases
{
    /// <summary>
    /// EF Core write-side repository base.
    /// <para>
    /// Concrete repositories inherit the standard CRUD lifetime here and may:
    /// <list type="bullet">
    ///   <item>override <c>GetByIdAsync</c> / <c>AddAsync</c> / ... when the aggregate needs
    ///   extra care (e.g. eager-loading navigations — see <c>PartyRepository</c>);</item>
    ///   <item>use <see cref="DbContext"/> for composed queries (Includes, projections)
    ///   that belong to the aggregate's own consistency.</item>
    /// </list>
    /// </para>
    /// </summary>
    internal abstract class Repository<T, TKey> : IRepository<T, TKey> where T : Entity<TKey>
    {
        /// <summary>The owning context, for derived repositories that need Includes or composed queries.</summary>
        protected readonly AppIdentityDbContext DbContext;

        /// <summary>The aggregate's own DbSet.</summary>
        protected readonly DbSet<T> Set;

        protected Repository(AppIdentityDbContext context)
        {
            DbContext = context;
            Set = context.Set<T>();
        }

        // `e.Id == id` is illegal on an unconstrained TKey (CS0019), and EqualityComparer<TKey>.Default.Equals is not
        // translatable by EF Core. Instance `.Equals(id)` IS translated correctly through the value converter.
        /// <summary>Virtual so aggregate repositories can override (e.g. to include aggregative navigations).</summary>
        public virtual async Task<T?> GetByIdAsync(TKey id)
            => await Set.FirstOrDefaultAsync(e => e.Id!.Equals(id));

        public virtual Task AddAsync(T model)
        {
            Set.Add(model);
            return Task.CompletedTask;
        }

        public virtual Task UpdateAsync(T model)
        {
            Set.Update(model);
            return Task.CompletedTask;
        }

        public virtual Task RemoveAsync(T model)
        {
            Set.Remove(model);
            return Task.CompletedTask;
        }
    }
}
