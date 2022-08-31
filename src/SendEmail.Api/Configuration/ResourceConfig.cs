using System.Globalization;
using System.Resources;
using SendEmail.Api.Resource;

namespace SendEmail.Api.Configuration;

public static class ResourceConfig
{
    public static IServiceCollection AddResourceConfiguration(this IServiceCollection services)
    {
        services.AddSingleton(new ResourceManager(typeof(SendEmailResource)));
        services.AddScoped(provider =>
        {
            var httpContext = provider.GetService<IHttpContextAccessor>()?.HttpContext;
            if (httpContext is null) return CultureInfo.InvariantCulture;
            return httpContext.Request.Headers.TryGetValue("language", out var language)
                ? new CultureInfo(language)
                : CultureInfo.InvariantCulture;
        });
        return services;
    }
}