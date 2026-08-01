using Application.Common.Interfaces;
using Domain.Bases;
using Domain.Entities.Accounting;
using Domain.Entities.Core;
using Domain.Entities.Expenses;
using Domain.Entities.Orders;
using Domain.Entities.Products;
using Domain.Entities.Purchasing;
using Infrastructure.Identity;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Contexts
{
    public partial class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        private readonly ICurrentUserContext? _userContext;

        public bool IgnoreBranchFilter { get; set; }

        private Guid? CurrentBrandId => _userContext?.ActiveBrandId;
        private Guid? CurrentBranchId => _userContext?.ActiveBranchId;
        private bool SkipBranchFilter =>
            IgnoreBranchFilter ||
            _userContext == null ||
            _userContext.CanAccessAllBranches;

        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserContext? userContext = null)
            : base(options)
        {
            _userContext = userContext;
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Batch> Batches { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public virtual DbSet<JournalEntry> JournalEntries { get; set; }
        public virtual DbSet<JournalEntryLine> JournalEntryLines { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<PurchaseItem> PurchaseItems { get; set; }
        public virtual DbSet<StockMovement> StockMovements { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Warehouse> Warehouses { get; set; }
        public virtual DbSet<WarehouseBatch> WarehouseBatches { get; set; }
        public virtual DbSet<WarehouseBranch> WarehouseBranches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ApplyScopeFilters(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.Ignore<IdentityPasskeyData>();
        }

        private void ApplyScopeFilters(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.IsOwned())
                    continue;

                var hasBrandId = entityType.FindProperty("BrandId") != null;
                var hasBranchId = entityType.FindProperty("BranchId") != null;

                if (!hasBrandId && !hasBranchId)
                    continue;

                var parameter = Expression.Parameter(entityType.ClrType, "e");
                Expression body = Expression.Constant(true);

                if (hasBrandId)
                {
                    var currentBrand = Expression.Property(Expression.Constant(this), nameof(CurrentBrandId));
                    var brandIsNotSet = Expression.Equal(currentBrand, Expression.Constant(null, typeof(Guid?)));
                    var entityBrand = Expression.Convert(Expression.Property(parameter, "BrandId"), typeof(Guid?));
                    var brandMatches = Expression.Equal(entityBrand, currentBrand);
                    body = Expression.AndAlso(body, Expression.OrElse(brandIsNotSet, brandMatches));
                }

                if (hasBranchId)
                {
                    var skipBranch = Expression.Property(Expression.Constant(this), nameof(SkipBranchFilter));
                    var currentBranch = Expression.Property(Expression.Constant(this), nameof(CurrentBranchId));
                    var entityBranch = Expression.Convert(Expression.Property(parameter, "BranchId"), typeof(Guid?));
                    var branchMatches = Expression.Equal(entityBranch, currentBranch);
                    body = Expression.AndAlso(body, Expression.OrElse(skipBranch, branchMatches));
                }

                var lambda = Expression.Lambda(body, parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            EnforceScopeOnChanges();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            EnforceScopeOnChanges();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void EnforceScopeOnChanges()
        {
            // لو مفيش مستخدم أو لسه بيعمل Login، عدي العمليات
            if (_userContext == null || _userContext.UserId == Guid.Empty)
                return;

            bool isCreatingNewBrand = ChangeTracker.Entries()
                .Any(e => e.Entity is Domain.Entities.Core.Brand && e.State == EntityState.Added);

            if (isCreatingNewBrand)
                return;

            foreach (var entry in ChangeTracker.Entries()
                         .Where(e => e.State is EntityState.Added or EntityState.Modified))
            {
                // التحقق فقط إذا كان الكيان يتبع الـ Multi-Tenancy
                if (entry.Entity is IMultiTenantEntity tenantEntity)
                {
                    if (entry.State == EntityState.Added && tenantEntity.BrandId == Guid.Empty)
                    {
                        tenantEntity.BrandId = _userContext.ActiveBrandId;
                    }
                    else if (_userContext.ActiveBrandId != Guid.Empty && tenantEntity.BrandId != _userContext.ActiveBrandId)
                    {
                        throw new UnauthorizedAccessException("Cross-brand write is forbidden.");
                    }
                }

                if (entry.Entity is IBranchScopedEntity branchEntity)
                {
                    if (_userContext.CanAccessAllBranches) continue;

                    if (!_userContext.ActiveBranchId.HasValue)
                        throw new UnauthorizedAccessException("Active branch is required.");

                    if (entry.State == EntityState.Added && branchEntity.BranchId == Guid.Empty)
                    {
                        branchEntity.BranchId = _userContext.ActiveBranchId.Value;
                    }
                    else if (branchEntity.BranchId != _userContext.ActiveBranchId)
                    {
                        throw new UnauthorizedAccessException("Cross-branch write is forbidden.");
                    }
                }
            }
        }

        private static Guid? GetGuidValue(object? value)
        {
            return value switch
            {
                Guid guid => guid,
                string s when Guid.TryParse(s, out var parsed) => parsed,
                _ => null
            };
        }
    }
}
