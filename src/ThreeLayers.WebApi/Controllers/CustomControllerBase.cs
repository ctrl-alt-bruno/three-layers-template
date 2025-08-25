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
    ProblemDetailsFactory problemDetailsFactory
) : ControllerBase
{
    protected ActionResult CreateCustomActionResult(ModelStateDictionary modelStateDictionary)
    {
        if (!modelStateDictionary.IsValid)
            foreach (ModelError error in modelStateDictionary.Values.SelectMany(x => x.Errors))
                Notify(error.Exception == null ? error.ErrorMessage : error.Exception.Message);

        return CreateCustomActionResult();
    }

    protected ActionResult CreateCustomActionResult(
        HttpStatusCode httpStatusCode = HttpStatusCode.OK,
        object? result = null
    )
    {
        if (notifier.HasNotification())
            return CreateErrorActionResult();

        return new ObjectResult(result) { StatusCode = Convert.ToInt32(httpStatusCode) };
    }

    protected ActionResult CreateCustomActionResult(
        string actionName,
        object routeValues,
        object result
    )
    {
        if (notifier.HasNotification())
            return CreateErrorActionResult();

        return CreatedAtAction(actionName, routeValues, result);
    }

    private ActionResult CreateErrorActionResult()
    {
        List<Notification> notifications = notifier.GetNotifications();

        // Determine the appropriate HTTP status code based on notification types
        HttpStatusCode statusCode = DetermineStatusCode(notifications);

        ValidationProblemDetails problemDetails = CreateProblemDetails(statusCode, notifications);

        return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
    }

    private HttpStatusCode DetermineStatusCode(List<Notification> notifications)
    {
        // Priority order: NotFound > Conflict > BusinessRule > Validation > BadRequest
        if (notifications.Any(n => n.Type == NotificationType.NotFound))
            return HttpStatusCode.NotFound;

        if (notifications.Any(n => n.Type == NotificationType.Conflict))
            return HttpStatusCode.Conflict;

        if (notifications.Any(n => n.Type == NotificationType.BusinessRule))
            return HttpStatusCode.UnprocessableEntity;

        if (notifications.Any(n => n.Type == NotificationType.Validation))
            return HttpStatusCode.BadRequest;

        return HttpStatusCode.BadRequest; // Default for BadRequest type
    }

    private ValidationProblemDetails CreateProblemDetails(
        HttpStatusCode statusCode,
        List<Notification> notifications
    )
    {
        string title = statusCode switch
        {
            HttpStatusCode.NotFound => "Not Found",
            HttpStatusCode.Conflict => "Conflict",
            HttpStatusCode.UnprocessableEntity => "Business Rule Violation",
            HttpStatusCode.BadRequest => "Bad Request",
            _ => "Error",
        };

        ValidationProblemDetails problemDetails =
            problemDetailsFactory.CreateValidationProblemDetails(
                HttpContext!,
                modelStateDictionary: new ModelStateDictionary(),
                statusCode: (int)statusCode,
                title: title,
                detail: "See 'errors' for details.",
                instance: HttpContext?.Request?.Path
            );

        // Group messages by notification type
        foreach (
            IGrouping<NotificationType, Notification> typeGroup in notifications.GroupBy(n =>
                n.Type
            )
        )
        {
            string key = typeGroup.Key.ToString().ToLowerInvariant();
            problemDetails.Errors.Add(key, typeGroup.Select(n => n.Message).ToArray());
        }

        return problemDetails;
    }

    protected void Notify(string message, NotificationType type = NotificationType.Validation)
    {
        notifier.Handle(new Notification(message, type));
    }

    protected void NotifyNotFound(string message)
    {
        notifier.Handle(new Notification(message, NotificationType.NotFound));
    }

    protected void NotifyConflict(string message)
    {
        notifier.Handle(new Notification(message, NotificationType.Conflict));
    }

    protected void NotifyBusinessRule(string message)
    {
        notifier.Handle(new Notification(message, NotificationType.BusinessRule));
    }
}
