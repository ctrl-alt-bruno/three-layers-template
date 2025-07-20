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
    protected ActionResult CustomResponse(ModelStateDictionary modelStateDictionary)
    {
        if (!modelStateDictionary.IsValid)
            foreach (ModelError error in modelStateDictionary.Values.SelectMany(x => x.Errors))
                notifier.Handle(new Notification(error.Exception == null
                    ? error.ErrorMessage
                    : error.Exception.Message));

        return CustomResponse();
    }

    protected ActionResult CustomResponse(HttpStatusCode httpStatusCode = HttpStatusCode.OK, object? result = null)
    {
        if (!notifier.HasNotification())
            return new ObjectResult(result)
            {
                StatusCode = Convert.ToInt32(httpStatusCode)
            };

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