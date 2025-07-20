using FluentValidation;

namespace ThreeLayers.Business.Models.Validation;

public class ProductValidation : AbstractValidator<Product>
{
    public ProductValidation()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The field '{PropertyName}' must be provided.")
            .Length(2, 200)
            .WithMessage("The field '{PropertyName}' must be between {MinLength} and {MaxLength} characters long.");

        RuleFor(c => c.Description)
            .NotEmpty().WithMessage("The field '{PropertyName}' must be provided.")
            .Length(2, 1000)
            .WithMessage("The field '{PropertyName}' must be between {MinLength} and {MaxLength} characters long.");

        RuleFor(c => c.Value)
            .GreaterThan(0).WithMessage("The field '{PropertyName}' must be greater than {ComparisonValue}.");
    }
}