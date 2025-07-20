using Microsoft.OpenApi.Models;

namespace ThreeLayers.WebAPI.Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ProtonSEC API",
                Version = "v1",
                Description = "API do projeto ProtonSEC usando arquitetura em três camadas"
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProtonSEC API v1");
            c.RoutePrefix = "swagger";
        });

        return app;
    }
}