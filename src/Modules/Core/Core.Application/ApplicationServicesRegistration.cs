using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddCoreApplicationServices(this IServiceCollection services)
    {
        // 1. تسجيل الـ MediatR (إذا كنت تستخدمه) لقراءة الـ Handlers تلقائياً
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // 2. تسجيل الـ FluentValidation (إذا كنت تستخدمه)
        // services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // 3. تسجيل أي خدمات (Services) عادية قمت بكتابتها داخل طبقة الـ Application
        // services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
