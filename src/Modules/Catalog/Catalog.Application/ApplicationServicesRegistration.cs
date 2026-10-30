using Catalog.Application.Behaviors;
using Catalog.Contracts.UseCases.CategoryCases;
using Catalog.Contracts.UseCases.ProductCases;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Catalog.Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddCatalogApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Validation must run BEFORE UnitOfWork to avoid unnecessary DB work
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

        // Pure contract use-case interfaces (Catalog.Contracts) implemented by pipeline adapters.
        services.AddTransient<IProductCases, Adapters.ProductCases.ProductMediatorCases>();
        services.AddTransient<ICategoryCases, Adapters.CategoryCases.CategoryMediatorCases>();

        return services;
    }
}