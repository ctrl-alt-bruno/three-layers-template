using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ThreeLayers.Data.Context;
using ThreeLayers.WebAPI.Configuration;
using ThreeLayers.WebAPI.Infrastructure.Errors;
using ThreeLayers.WebAPI.Infrastructure.Logs;
using ThreeLayers.WebAPI.Infrastructure.Middleware;

namespace ThreeLayers.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Host.UseSerilog();
            
            builder.Services.AddAuthorization();

            builder.Services.AddSwaggerDocumentation();

            builder.Services.AddDbContext<MyDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.ConfigureDependencies();

            builder.Services.AddControllers();

            builder.Services.AddSingleton<ProblemDetailsFactory, CustomProblemDetailsFactory>();
        }

        WebApplication app = builder.Build();
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
            
            app.UseCustomExceptionHandler();

            if (app.Environment.IsDevelopment())
                app.UseSwaggerDocumentation();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        
        Log.CloseAndFlush();
    }
}