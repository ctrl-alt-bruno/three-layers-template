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