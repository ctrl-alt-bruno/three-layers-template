using FluentValidation;
using FluentValidation.Results;
using ThreeLayers.Business.Exceptions;
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

    protected void ValidateAndThrow<TValidation, TEntity>(TValidation validation, TEntity? entity)
        where TValidation : AbstractValidator<TEntity>
        where TEntity : Entity
    {
        if (entity == null)
            throw new Exceptions.ValidationException("Entity cannot be null.");

        ValidationResult validationResult = validation.Validate(entity);
            
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            throw new Exceptions.ValidationException(errors);
        }
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