using FluentValidation;

namespace ThreeLayers.Business.Models.Validation;

public class ProductValidation : AbstractValidator<Product>
{
    public ProductValidation()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("The field '{PropertyName}' is required.")
            .Length(2, 200)
            .WithMessage(
                "The field '{PropertyName}' must be between {MinLength} and {MaxLength} characters long."
            );

        RuleFor(c => c.Description)
            .NotEmpty()
            .WithMessage("The field '{PropertyName}' is required.")
            .Length(2, 1000)
            .WithMessage(
                "The field '{PropertyName}' must be between {MinLength} and {MaxLength} characters long."
            );

        RuleFor(c => c.Value)
            .GreaterThan(0)
            .WithMessage("The field '{PropertyName}' must be greater than {ComparisonValue}.");

        RuleFor(c => c.SupplierId)
            .NotEmpty()
            .WithMessage("The field '{PropertyName}' is required.")
            .NotEqual(Guid.Empty)
            .WithMessage("The field '{PropertyName}' must be a valid GUID.");

        RuleFor(c => c.CreationDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("The field '{PropertyName}' cannot be in the future.");
    }
}
