using AutoMapper;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using SendEmail.Data;

namespace SendEmail.Api.Configuration;

public static class ApiConfig
{
    public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddHealthChecks().AddDbContextCheck<SqlContext>();
            
        services.AddDbContext<SqlContext>(options =>
        {
            // options
            //     .LogTo(Console.WriteLine, LogLevel.Information)
            //     .EnableSensitiveDataLogging(
            //         AspnetcoreEnvironment.IsCurrentAspnetcoreEnvironmentValue(AspnetcoreEnvironmentEnum.Docker)
            //         || AspnetcoreEnvironment.IsCurrentAspnetcoreEnvironmentValue(AspnetcoreEnvironmentEnum
            //             .Development));
            options
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery));
        });
            
        services.AddMemoryCache();

        services.AddAutoMapper(typeof(Startup));

        services.AddHttpContextAccessor();

        services.AddCors(options =>
        {
            options.AddPolicy("Total",
                builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
        });
    }
    
    public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
                        
        app.UseRouting();

        app.UseCors("Total");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health/startup");
            endpoints.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
            endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = _ => false });
            endpoints.MapControllers();
        });

        return app;
    }
}