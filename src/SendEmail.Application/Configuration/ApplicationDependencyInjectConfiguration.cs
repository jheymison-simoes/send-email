using Microsoft.Extensions.DependencyInjection;
using SendEmail.Application.Interfaces.Services;
using SendEmail.Application.Models;
using SendEmail.Application.Services;

namespace SendEmail.Application.Configuration;

public static class ApplicationDependencyInjectConfiguration
{
    public static void DependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<SendEmailModelValidator>();
        services.AddScoped<IEmailManageService, EmailManageService>();
    }
}