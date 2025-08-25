using FluentValidation;
using FluentValidation.Results;
using ThreeLayers.Business.Interfaces;
using ThreeLayers.Business.Models;
using ThreeLayers.Business.Notifications;

namespace ThreeLayers.Business.Services;

public abstract class BaseService(INotifier notifier)
{
    protected bool Validate<TValidation, TEntity>(TValidation validation, TEntity? entity)
        where TValidation : AbstractValidator<TEntity>
        where TEntity : Entity
    {
        if (entity == null)
        {
            Notify("Entity cannot be null", NotificationType.BadRequest);
            return false;
        }

        ValidationResult validationResult = validation.Validate(entity);
            
        if (!validationResult.IsValid)
            Notify(validationResult);

        return validationResult.IsValid;
    }

    private void Notify(ValidationResult validationResult)
    {
        foreach (ValidationFailure? error in validationResult.Errors)
            Notify(error.ErrorMessage, NotificationType.Validation);
    }

    protected void Notify(string message, NotificationType type = NotificationType.Validation)
    {
        notifier.Handle(new Notification(message, type));
    }

    protected void NotifyNotFound(string entityName)
    {
        Notify($"{entityName} not found", NotificationType.NotFound);
    }

    protected void NotifyBusinessRule(string message)
    {
        Notify(message, NotificationType.BusinessRule);
    }

    protected void NotifyConflict(string message)
    {
        Notify(message, NotificationType.Conflict);
    }

    protected bool HasNotifications()
    {
        return notifier.HasNotification();
    }

    protected bool HasNotificationsOfType(NotificationType type)
    {
        return notifier.HasNotificationOfType(type);
    }
}