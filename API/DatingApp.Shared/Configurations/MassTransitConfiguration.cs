using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.Shared.Configurations;

public static class MassTransitConfiguration
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumers(assemblies);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}