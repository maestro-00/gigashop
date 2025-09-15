using System.Reflection;
using BuildingBlocks.Behaviours;
using Microsoft.Extensions.DependencyInjection;

namespace Order.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(opt =>
        {
            opt.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            opt.AddOpenBehavior(typeof(LoggingBehaviour<,>));
            opt.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });
        return services;
    }
}