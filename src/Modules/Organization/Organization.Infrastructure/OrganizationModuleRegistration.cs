using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Organization.Application;

namespace Organization.Infrastructure;

public static class OrganizationModuleRegistration
{
    public static IServiceCollection AddOrganizationModule(this IServiceCollection services, IConfiguration configuration)
    {
        #region Application Services Registrations
        services.AddOrganizationApplicationServices();
        #endregion

        // TODO: Register the module DbContext (OrganizationDbContext) here,
        // following the InventoryModuleRegistration / IdentityModuleRegistration pattern,
        // e.g.:
        //   string connectionString = configuration.GetConnectionString("OrganizationsConnection")
        //       ?? throw new ArgumentNullException("Connection String is null");
        //   services.AddDbContext<OrganizationDbContext>(options => options.UseSqlServer(connectionString));
        //
        // TODO: Register UnitOfWork, command/query repositories, and module services.

        return services;
    }
}
