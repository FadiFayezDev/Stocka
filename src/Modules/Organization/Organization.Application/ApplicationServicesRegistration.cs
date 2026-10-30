using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Organization.Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddOrganizationApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // TODO: Register pipeline behaviors (ValidationBehavior, UnitOfWorkBehavior)
        // and contract use-case adapters once use-cases are added.

        return services;
    }
}
