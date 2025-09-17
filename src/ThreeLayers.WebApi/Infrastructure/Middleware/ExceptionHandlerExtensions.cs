using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace ThreeLayers.WebAPI.Infrastructure.Middleware;

public static class ExceptionHandlerExtensions
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                ILogger<Program> logger = context.RequestServices.GetRequiredService<
                    ILogger<Program>
                >();
                ProblemDetailsFactory factory =
                    context.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                HttpRequest request = context.Request;
                Exception? exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                string traceId = context.TraceIdentifier;
                string? ip = context.Connection.RemoteIpAddress?.ToString();
                string userAgent = request.Headers["User-Agent"].ToString();
                Dictionary<string, string> headers = request.Headers.ToDictionary(
                    h => h.Key,
                    h => h.Value.ToString()
                );
                request.Headers.TryGetValue("X-Correlation-ID", out StringValues correlationId);

                using (LogContext.PushProperty("TraceId", traceId))
                using (LogContext.PushProperty("ClientIp", ip))
                using (LogContext.PushProperty("UserAgent", userAgent))
                using (LogContext.PushProperty("Headers", headers, destructureObjects: true))
                using (LogContext.PushProperty("CorrelationId", correlationId.ToString()))
                {
                    ProblemDetails problem = factory.CreateProblemDetails(
                        context,
                        statusCode: 500,
                        title: "Internal Server Error",
                        detail: "An unexpected error occurred."
                    );

                    context.Response.StatusCode = problem.Status ?? 500;
                    context.Response.ContentType = "application/problem+json";
                    await context.Response.WriteAsJsonAsync(problem);
                }
            });
        });
    }
}
