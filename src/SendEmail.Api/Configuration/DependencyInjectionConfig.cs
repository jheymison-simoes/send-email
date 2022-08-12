using System.Globalization;
using System.Resources;
using SendEmail.Api.Resource;
using SendEmail.Application.Configuration;
using SendEmail.Data.Configuration;

namespace SendEmail.Api.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection DependencyInjection(this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddSingleton(new ResourceManager(typeof(SendEmailResource)));
        services.AddScoped(provider =>
        {
            var httpContext = provider.GetService<IHttpContextAccessor>()?.HttpContext;

            if (httpContext == null)
            {
                return CultureInfo.InvariantCulture;
            }

            return httpContext.Request.Headers.TryGetValue("language", out var language)
                ? new CultureInfo(language)
                : CultureInfo.InvariantCulture;
        });

        DataInjectionConfiguration.DependencyInjection(services);
        ApplicationDependencyInjectConfiguration.DependencyInjection(services);
   
        return services;
    }
}