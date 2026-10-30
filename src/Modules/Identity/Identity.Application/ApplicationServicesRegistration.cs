using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddIdentityApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationServicesRegistration).Assembly));
            return services;
        }
    }
}
