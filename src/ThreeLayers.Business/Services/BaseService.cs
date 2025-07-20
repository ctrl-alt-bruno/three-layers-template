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
            return false;

        ValidationResult validationResult = validation.Validate(entity);
            
        if (!validationResult.IsValid)
            Notify(validationResult);

        return validationResult.IsValid;
    }

    private void Notify(ValidationResult validationResult)
    {
        foreach (ValidationFailure? error in validationResult.Errors)
            Notify(error.ErrorMessage);
    }

    protected void Notify(string message)
    {
        notifier.Handle(new Notification(message));
    }
}