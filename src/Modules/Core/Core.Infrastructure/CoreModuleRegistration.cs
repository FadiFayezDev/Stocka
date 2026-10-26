using BuildingBlocks.Application.Common.Interfaces;
using Core.Application;
using Core.Application.Common.Interfaces;
using Core.Application.Repositories.Queries.Brand;
using Core.Infrastructure.Contexts;
using Core.Infrastructure.Repositories.Commands.Bases;
using Core.Infrastructure.Repositories.Queries;
using Core.Infrastructure.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Core.Infrastructure;

public static class CoreModuleRegistration
{
    public static IServiceCollection AddCoreModule(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("ConnectionString Is Nul");

        services.AddDbContext<CoreDbContext>(option =>
        {
            option.UseSqlServer(connectionString);
        });

        #region Dapper registration for IDbConnection
        services.AddTransient<IDbConnection>(sp =>
            new SqlConnection(connectionString)
        );
        #endregion

        #region Application Services Registrations
        services.AddCoreApplicationServices();
        #endregion

        #region Repository Registrations
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<Application.Repositories.Commands.IBrandRepository, Repositories.Commands.BrandRepository>();
        #endregion

        #region Query Repository Registrations
        services.AddScoped<IBrandQueryRepository, BrandQueryRepository>();
        #endregion

        #region Service Registrations
        services.AddScoped<ICoreUnitOfWork, UnitOfWork>();
        //services.AddScoped<Inventory.Contracts.Services.IInventoryServices, Inventory.Application.Services.InventoryServices>();
        #endregion
        return services;
    }
}