using Microsoft.Extensions.DependencyInjection;

namespace SendEmail.Data.Configuration;

public static class DataInjectionConfiguration
{
    public static IServiceCollection DependencyInjection(this IServiceCollection services)
    {
        services = InjectionDependencyRepository(services);
        services = InjectionDependencyUniOfWork(services);
        return services;
    }

    private static IServiceCollection InjectionDependencyRepository(IServiceCollection services)
    {
        
        return services;
    }
    
    private static IServiceCollection InjectionDependencyUniOfWork(IServiceCollection services)
    {
        return services;
    }
}