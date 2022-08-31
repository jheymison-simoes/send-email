using Microsoft.Extensions.DependencyInjection;
using SendEmail.Application.Services;
using SendEmail.Business.Interfaces.Services;
using SendEmail.Business.Models;
using SendEmail.Business.ServiceModels;

namespace SendEmail.Application.Configuration;

public static class ApplicationDependencyInjectConfiguration
{
    public static void DependencyInjection(this IServiceCollection services)
    {
        #region Validators
        services.AddScoped<SendEmailModelValidator>();
        services.AddScoped<LogEmailValidator>();
        #endregion

        #region Services
        services.AddScoped<IEmailManagerService, EmailManageService>();
        #endregion
        
    }
}