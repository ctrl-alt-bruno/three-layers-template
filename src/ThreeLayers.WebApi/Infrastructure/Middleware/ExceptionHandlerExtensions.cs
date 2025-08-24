using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;
using Serilog.Context;
using ThreeLayers.Business.Exceptions;
using System.Net;

namespace ThreeLayers.WebAPI.Infrastructure.Middleware;

public static class ExceptionHandlerExtensions
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                ILogger<Program> logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                ProblemDetailsFactory factory = context.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                HttpRequest request = context.Request;
                Exception? exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                string traceId = context.TraceIdentifier;
                string? ip = context.Connection.RemoteIpAddress?.ToString();
                string userAgent = request.Headers["User-Agent"].ToString();
                Dictionary<string, string> headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());
                request.Headers.TryGetValue("X-Correlation-ID", out StringValues correlationId);

                // Determine the appropriate response based on exception type
                (int statusCode, string title, string detail, object? extensions) = GetErrorResponse(exception);

                using (LogContext.PushProperty("TraceId", traceId))
                using (LogContext.PushProperty("ClientIp", ip))
                using (LogContext.PushProperty("UserAgent", userAgent))
                using (LogContext.PushProperty("Headers", headers, destructureObjects: true))
                using (LogContext.PushProperty("CorrelationId", correlationId.ToString()))
                {
                    // Log exception with appropriate level based on status code
                    if (statusCode >= 500)
                    {
                        logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}", traceId);
                    }
                    else if (statusCode >= 400)
                    {
                        logger.LogWarning(exception, "Client error occurred. TraceId: {TraceId}, StatusCode: {StatusCode}", traceId, statusCode);
                    }

                    ProblemDetails problem = factory.CreateProblemDetails(
                        context,
                        statusCode: statusCode,
                        title: title,
                        detail: detail
                    );

                    // Add additional properties for specific exception types
                    if (extensions != null && problem.Extensions != null)
                    {
                        foreach (var property in extensions.GetType().GetProperties())
                        {
                            var value = property.GetValue(extensions);
                            if (value != null)
                            {
                                problem.Extensions[property.Name] = value;
                            }
                        }
                    }

                    context.Response.StatusCode = problem.Status ?? statusCode;
                    context.Response.ContentType = "application/problem+json";
                    await context.Response.WriteAsJsonAsync(problem);
                }
            });
        });
    }

    private static (int statusCode, string title, string detail, object? extensions) GetErrorResponse(Exception? exception)
    {
        return exception switch
        {
            EntityNotFoundException entityNotFound => (
                statusCode: (int)HttpStatusCode.NotFound,
                title: "Not Found",
                detail: entityNotFound.Message,
                extensions: new { EntityName = entityNotFound.EntityName, EntityId = entityNotFound.EntityId }
            ),
            BusinessRuleException businessRule => (
                statusCode: (int)HttpStatusCode.Conflict,
                title: "Business Rule Violation",
                detail: businessRule.Message,
                extensions: (object?)null
            ),
            ValidationException validation => (
                statusCode: (int)HttpStatusCode.UnprocessableEntity,
                title: "Validation Failed",
                detail: validation.Message,
                extensions: new { Errors = validation.Errors }
            ),
            UnauthorizedException unauthorized => (
                statusCode: (int)HttpStatusCode.Unauthorized,
                title: "Unauthorized",
                detail: unauthorized.Message,
                extensions: (object?)null
            ),
            ForbiddenException forbidden => (
                statusCode: (int)HttpStatusCode.Forbidden,
                title: "Forbidden",
                detail: forbidden.Message,
                extensions: (object?)null
            ),
            ArgumentException or ArgumentNullException => (
                statusCode: (int)HttpStatusCode.BadRequest,
                title: "Bad Request",
                detail: exception.Message,
                extensions: (object?)null
            ),
            _ => (
                statusCode: (int)HttpStatusCode.InternalServerError,
                title: "Internal Server Error",
                detail: "An unexpected error occurred.",
                extensions: (object?)null
            )
        };
    }
}