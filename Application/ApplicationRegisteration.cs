using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationRegisteration
    {
        public static void AddApplicationRegisteration(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(ApplicationRegisteration).Assembly);
            });
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationRegisteration).Assembly));
        }
    }
}
