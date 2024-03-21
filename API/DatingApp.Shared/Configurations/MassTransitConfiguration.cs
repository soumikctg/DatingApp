using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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