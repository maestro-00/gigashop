using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Data;
using Order.Infrastructure.Data;
using Order.Infrastructure.Data.Interceptors;

namespace Order.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        //Add Ef Core Configs
        services.AddDbContext<OrderDbContext>((sp,opt) =>
        {
            opt.AddInterceptors(sp.GetService<ISaveChangesInterceptor>()!); 
            opt.UseSqlServer(configuration.GetConnectionString("Database"));
        });
        
        services.AddScoped<IOrderDbContext, OrderDbContext>();
        return services;
    }
}