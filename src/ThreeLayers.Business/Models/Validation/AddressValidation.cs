using FluentValidation;

namespace ThreeLayers.Business.Models.Validation;

public class AddressValidation : AbstractValidator<Address>
{
    public AddressValidation()
    {
        RuleFor(c => c.Street)
            .NotEmpty().WithMessage("The field {PropertyName} is required.")
            .Length(2, 200).WithMessage("The field {PropertyName} must be between {MinLength} and {MaxLength} characters long.");

        RuleFor(c => c.Number)
            .NotEmpty().WithMessage("The field {PropertyName} is required.")
            .Length(1, 50).WithMessage("The field {PropertyName} must be between {MinLength} and {MaxLength} characters long.");

        RuleFor(c => c.PostalCode)
            .NotEmpty().WithMessage("The field {PropertyName} is required.")
            .Matches(@"^\d{5}-?\d{3}$").WithMessage("The field {PropertyName} must be a valid postal code (e.g., 12345-678 or 12345678).");

        RuleFor(c => c.Region)
            .NotEmpty().WithMessage("The field {PropertyName} is required.")
            .Length(2, 100).WithMessage("The field {PropertyName} must be between {MinLength} and {MaxLength} characters long.");

        RuleFor(c => c.City)
            .NotEmpty().WithMessage("The field {PropertyName} is required.")
            .Length(2, 100).WithMessage("The field {PropertyName} must be between {MinLength} and {MaxLength} characters long.");

        RuleFor(c => c.State)
            .NotEmpty().WithMessage("The field {PropertyName} is required.")
            .Length(2, 50).WithMessage("The field {PropertyName} must be between {MinLength} and {MaxLength} characters long.");

        When(c => !string.IsNullOrEmpty(c.Complement), () => 
        {
            RuleFor(c => c.Complement)
                .MaximumLength(100).WithMessage("The field {PropertyName} must not exceed {MaxLength} characters.");
        });
    }
}