using BuildingBlocks.Domain.Bases;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Contexts
{
    public class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreDbContext).Assembly);
        }

        public DbSet<Brand> Brands { get; set; }
        //public DbSet<Customer> Customers { get; set; }
        //public DbSet<Employee> Employees { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DispatchDomainEvents();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            DispatchDomainEvents();
            return base.SaveChanges();
        }

        private void DispatchDomainEvents()
        {
            var aggregates = ChangeTracker.Entries<IAggregateRoot>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();
            aggregates.ForEach(a => a.ClearDomainEvents());
        }
    }
}