using Catalog.Application;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Repositories.Commands;
using Catalog.Application.Repositories.Commands.Bases;
using Catalog.Application.Repositories.Queries.Category;
using Catalog.Application.Repositories.Queries.Product;
using Catalog.Infrastructure.Contexts;
using Catalog.Infrastructure.Repositories.Commands;
using Catalog.Infrastructure.Repositories.Commands.Bases;
using Catalog.Infrastructure.Repositories.Queries;
using Catalog.Infrastructure.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;


namespace Catalog.Infrastructure;

public static class CatalogModuleRegistration
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("ConnectionString Is Nul");

        services.AddDbContext<CatalogDbContext>(option =>
        {
            option.UseSqlServer(connectionString);
        });

        #region Dapper registration for IDbConnection
        services.AddTransient<IDbConnection>(sp =>
            new SqlConnection(connectionString)
        );
        #endregion

        #region Application Services Registrations
        services.AddCatalogApplicationServices();
        #endregion

        #region Repository Registrations
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        #endregion

        #region Query Repository Registrations
        services.AddScoped<IProductQueryRepository, ProductQueryRepository>();
        services.AddScoped<ICategoryQueryRepository, CategoryQueryRepository>();
        #endregion

        #region Service Registrations
        services.AddScoped<ICatalogUnitOfWork, UnitOfWork>();
        #endregion

        return services;
    }
}