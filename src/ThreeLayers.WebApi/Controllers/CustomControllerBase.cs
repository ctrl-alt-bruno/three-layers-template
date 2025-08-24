using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ThreeLayers.Business.Interfaces;
using ThreeLayers.Business.Notifications;

namespace ThreeLayers.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class CustomControllerBase(
    INotifier notifier,
    ILogger<CustomControllerBase> logger,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    protected ActionResult CreateCustomActionResult(ModelStateDictionary modelStateDictionary)
    {
        if (!modelStateDictionary.IsValid)
            foreach (ModelError error in modelStateDictionary.Values.SelectMany(x => x.Errors))
                Notify(error.Exception == null ? error.ErrorMessage : error.Exception.Message);

        return CreateCustomActionResult();
    }

    protected ActionResult CreateCustomActionResult(HttpStatusCode httpStatusCode = HttpStatusCode.OK, object? result = null)
    {
        if (notifier.HasNotification())
            return CreateBadRequestObjectResult();
        
        return new ObjectResult(result)
        {
            StatusCode = Convert.ToInt32(httpStatusCode)
        };
    }
    
    protected ActionResult CreateCustomActionResult(string actionName, object routeValues, object result)
    {
        if (notifier.HasNotification())
            return CreateBadRequestObjectResult();
        
        return CreatedAtAction(
            actionName,
            routeValues,
            result
        );
    }

    protected ActionResult CreateNotFoundResult(string? message = null)
    {
        ProblemDetails problemDetails = problemDetailsFactory.CreateProblemDetails(
            HttpContext!,
            statusCode: (int)HttpStatusCode.NotFound,
            title: "Not Found",
            detail: message ?? "The requested resource was not found."
        );

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    protected ActionResult CreateConflictResult(string? message = null)
    {
        ProblemDetails problemDetails = problemDetailsFactory.CreateProblemDetails(
            HttpContext!,
            statusCode: (int)HttpStatusCode.Conflict,
            title: "Conflict",
            detail: message ?? "The request conflicts with the current state of the resource."
        );

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    protected ActionResult CreateUnprocessableEntityResult(IEnumerable<string>? errors = null)
    {
        ValidationProblemDetails problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
            HttpContext!,
            modelStateDictionary: new ModelStateDictionary(),
            statusCode: (int)HttpStatusCode.UnprocessableEntity,
            title: "Unprocessable Entity",
            detail: "The request was well-formed but contains semantic errors."
        );

        if (errors != null)
            problemDetails.Errors.Add("validationErrors", errors.ToArray());

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    protected ActionResult CreateUnauthorizedResult(string? message = null)
    {
        ProblemDetails problemDetails = problemDetailsFactory.CreateProblemDetails(
            HttpContext!,
            statusCode: (int)HttpStatusCode.Unauthorized,
            title: "Unauthorized",
            detail: message ?? "Authentication is required to access this resource."
        );

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    protected ActionResult CreateForbiddenResult(string? message = null)
    {
        ProblemDetails problemDetails = problemDetailsFactory.CreateProblemDetails(
            HttpContext!,
            statusCode: (int)HttpStatusCode.Forbidden,
            title: "Forbidden",
            detail: message ?? "You do not have permission to access this resource."
        );

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    private ActionResult CreateBadRequestObjectResult()
    {
        ValidationProblemDetails problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
            HttpContext!,
            modelStateDictionary: new ModelStateDictionary(),
            statusCode: (int)HttpStatusCode.BadRequest,
            title: "Bad Request",
            detail: "See 'errors' for details.",
            instance: HttpContext?.Request?.Path
        );

        problemDetails.Errors.Add("messages", notifier.GetNotifications().Select(n => n.Message).ToArray());

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    protected void Notify(string message)
    {
        notifier.Handle(new Notification(message));
    }
}