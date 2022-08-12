using SendEmail.Api.Configuration;
using SendEmail.Data;

namespace SendEmail.Api;

public class Startup
{
    private IConfiguration Configuration { get; }
        
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApiConfiguration(Configuration);
        services.DependencyInjection(Configuration);
        services.AddSwaggerConfiguration();
        services.Configure<CredentialsGmailApi>(Configuration.GetSection("GmailApiCredentials"));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SqlContext context)
    {
        app.UseApiConfiguration(env);
        app.UseSwaggerConfiguration();
    }
}
