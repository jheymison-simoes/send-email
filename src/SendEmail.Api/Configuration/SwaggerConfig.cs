using Microsoft.OpenApi.Models;

namespace SendEmail.Api.Configuration;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Project API Your Sport",
                Description = "API Your Sport",
                Contact = new OpenApiContact() {Name = "Jheymison", Email = "jheymis1574@gmail.com"}
            });
                
            // c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            // {
            //     Description = "Enter the JWT token like this: Bearer {your token}",
            //     Name = "Authorization",
            //     Scheme = "Bearer",
            //     BearerFormat = "JWT",
            //     In = ParameterLocation.Header,
            //     Type = SecuritySchemeType.ApiKey
            // });
            // c.AddSecurityRequirement(new OpenApiSecurityRequirement
            // {
            //     {
            //         new OpenApiSecurityScheme
            //         {
            //             Reference = new OpenApiReference
            //             {
            //                 Type = ReferenceType.SecurityScheme,
            //                 Id = "Bearer"
            //             }
            //         },
            //         new string[] {}
            //     }
            // });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SendEmail.Api v1"));

        return app;
    }
}