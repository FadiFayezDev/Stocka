using Application.Common.Interfaces;
using Application.QueryRepositories;
using Domain.Contracts;
using Domain.Repositories.Commands;
using Infrastructure.Bases;
using Infrastructure.Contexts;
using Infrastructure.Helpers;
using Infrastructure.Identity;
using Infrastructure.Repositories.Commands;
using Infrastructure.Repositories.Queries;
using Infrastructure.Serviecs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;

namespace Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static void AddInfrastructureRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            // Validate the connection string
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty.");

            #region Entity Framework Core registration
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
            });
            #endregion

            #region Dapper registration for IDbConnection
            services.AddScoped<IDbConnection>(sp =>
                new NpgsqlConnection(connectionString)
            );
            #endregion

            #region Identity services registration
            services.Configure<JWT>(configuration.GetSection("JWT"));
            #endregion

            #region Application services registration
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IMediaStorage, MediaStorage>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion

            #region Command Repositories
            services.AddScoped<IAccountCommandRepository, AccountCommandRepository>();
            services.AddScoped<IJournalEntryCommandRepository, JournalEntryCommandRepository>();
            services.AddScoped<IJournalEntryLineCommandRepository, JournalEntryLineCommandRepository>();
            services.AddScoped<IBranchCommandRepository, BranchCommandRepository>();
            services.AddScoped<IBrandCommandRepository, BrandCommandRepository>();
            services.AddScoped<IBatchCommandRepository, BatchCommandRepository>();
            services.AddScoped<IProductCommandRepository, ProductCommandRepository>();
            services.AddScoped<IProductCategoryCommandRepository, ProductCategoryCommandRepository>();
            services.AddScoped<IWarehouseCommandRepository, WarehouseCommandRepository>();
            services.AddScoped<ISupplierCommandRepository, SupplierCommandRepository>();
            services.AddScoped<IPurchaseCommandRepository, PurchaseCommandRepository>();
            services.AddScoped<IPurchaseItemCommandRepository, PurchaseItemCommandRepository>();
            services.AddScoped<IOrderCommandRepository, OrderCommandRepository>();
            services.AddScoped<IOrderItemCommandRepository, OrderItemCommandRepository>();
            services.AddScoped<IExpenseCommandRepository, ExpenseCommandRepository>();
            services.AddScoped<IExpenseCategoryCommandRepository, ExpenseCategoryCommandRepository>();
            services.AddScoped<IStockMovementCommandRepository, StockMovementCommandRepository>();
            services.AddScoped<IWarehouseBatchCommandRepository, WarehouseBatchCommandRepository>();
            services.AddScoped<ICustomerCommandRepository, CustomerCommandRepository>();
            services.AddScoped<IEmployeeCommandRepository, EmployeeCommandRepository>();
            #endregion

            #region Query Repositories
            services.AddScoped<IAccountQueryRepository, AccountQueryRepository>();
            services.AddScoped<IJournalEntryQueryRepository, JournalEntryQueryRepository>();
            services.AddScoped<IJournalEntryLineQueryRepository, JournalEntryLineQueryRepository>();
            services.AddScoped<IBranchQueryRepository, BranchQueryRepository>();
            services.AddScoped<IBrandQueryRepository, BrandQueryRepository>();
            services.AddScoped<IBatchQueryRepository, BatchQueryRepository>();
            services.AddScoped<IProductQueryRepository, ProductQueryRepository>();
            services.AddScoped<IProductCategoryQueryRepository, ProductCategoryQueryRepository>();
            services.AddScoped<IWarehouseQueryRepository, WarehouseQueryRepository>();
            services.AddScoped<ISupplierQueryRepository, SupplierQueryRepository>();
            services.AddScoped<IPurchaseQueryRepository, PurchaseQueryRepository>();
            services.AddScoped<IPurchaseItemQueryRepository, PurchaseItemQueryRepository>();
            services.AddScoped<IOrderQueryRepository, OrderQueryRepository>();
            services.AddScoped<IOrderItemQueryRepository, OrderItemQueryRepository>();
            services.AddScoped<IExpenseQueryRepository, ExpenseQueryRepository>();
            services.AddScoped<IExpenseCategoryQueryRepository, ExpenseCategoryQueryRepository>();
            services.AddScoped<IStockMovementQueryRepository, StockMovementQueryRepository>();
            services.AddScoped<IWarehouseBatchQueryRepository, WarehouseBatchQueryRepository>();
            services.AddScoped<ICustomerQueryRepository, CustomerQueryRepository>();
            services.AddScoped<IEmployeeQueryRepository, EmployeeQueryRepository>();
            #endregion
        }
    }
}