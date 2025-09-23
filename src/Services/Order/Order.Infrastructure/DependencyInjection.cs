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

        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        //Add Ef Core Configs
        services.AddDbContext<OrderDbContext>((sp,opt) =>
        { 
            var interceptors = sp.GetServices<ISaveChangesInterceptor>();
            opt.AddInterceptors(interceptors);
            opt.UseSqlServer(configuration.GetConnectionString("Database"));
        });
        
        services.AddScoped<IOrderDbContext, OrderDbContext>();
        return services;
    }
}