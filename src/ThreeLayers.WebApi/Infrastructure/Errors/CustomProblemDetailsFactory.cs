using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace ThreeLayers.WebAPI.Infrastructure.Errors;

public class CustomProblemDetailsFactory(IOptions<ApiBehaviorOptions> options)
    : ProblemDetailsFactory
{
    private readonly ApiBehaviorOptions _options =
        options?.Value ?? throw new ArgumentNullException(nameof(options));

    public override ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null
    )
    {
        statusCode ??= 500;

        ProblemDetails problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title ?? ReasonPhrases.GetReasonPhrase(statusCode.Value),
            Type = type,
            Detail = detail,
            Instance = instance ?? httpContext?.Request?.Path,
        };

        ApplyDefaults(httpContext, problemDetails, statusCode.Value);
        return problemDetails;
    }

    public override ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext httpContext,
        ModelStateDictionary modelStateDictionary,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null
    )
    {
        if (modelStateDictionary == null)
        {
            throw new ArgumentNullException(nameof(modelStateDictionary));
        }

        statusCode ??= 400;

        ValidationProblemDetails validationProblemDetails = new ValidationProblemDetails(
            modelStateDictionary
        )
        {
            Status = statusCode,
            Type = type,
            Detail = detail,
            Instance = instance ?? httpContext?.Request?.Path,
            Title = title ?? "Bad Request",
        };

        ApplyDefaults(httpContext, validationProblemDetails, statusCode.Value);
        return validationProblemDetails;
    }

    private void ApplyDefaults(
        HttpContext httpContext,
        ProblemDetails problemDetails,
        int statusCode
    )
    {
        problemDetails.Status ??= statusCode;

        if (
            _options.ClientErrorMapping.TryGetValue(
                statusCode,
                out ClientErrorData? clientErrorData
            )
        )
        {
            problemDetails.Title ??= clientErrorData.Title;
            problemDetails.Type ??= clientErrorData.Link;
        }

        problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        problemDetails.Extensions["timestamp"] = DateTimeOffset.UtcNow;
    }
}
