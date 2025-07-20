using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace ThreeLayers.WebAPI.Infrastructure.Logs;

public class RequestLoggingMiddleware(
    RequestDelegate next, 
    ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        HttpRequest request = context.Request;
        string traceId = context.TraceIdentifier;
        string? ip = context.Connection.RemoteIpAddress?.ToString();
        string userAgent = request.Headers["User-Agent"].ToString();
        string? contentType = request.ContentType;
        string? queryString = request.QueryString.HasValue ? request.QueryString.Value : string.Empty;
        Dictionary<string, string> headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());

        // Correlation ID (se enviado por header)
        string? correlationId = null;
        if (request.Headers.TryGetValue("X-Correlation-ID", out StringValues cid))
        {
            correlationId = cid.ToString();
        }

        using (LogContext.PushProperty("TraceId", traceId))
        using (LogContext.PushProperty("ClientIp", ip))
        using (LogContext.PushProperty("UserAgent", userAgent))
        using (LogContext.PushProperty("Headers", headers, destructureObjects: true))
        using (LogContext.PushProperty("ContentType", contentType))
        using (LogContext.PushProperty("QueryString", queryString))
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            logger.LogInformation("Incoming request: {Method} {Path}", request.Method, request.Path);

            await next(context);

            logger.LogInformation("Response: {StatusCode}", context.Response.StatusCode);
        }
    }
}