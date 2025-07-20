using FluentValidation;

namespace ThreeLayers.Business.Models.Validation;

public class AddressValidation : AbstractValidator<Address>
{
    public AddressValidation()
    {
        RuleFor(c => c.Street)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido.")
            .Length(2, 200).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

        RuleFor(c => c.Region)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido.")
            .Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

        RuleFor(c => c.PostalCode)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido.")
            .Length(8).WithMessage("O campo {PropertyName} precisa ter {MaxLength} caracteres.");

        RuleFor(c => c.Street)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido.")
            .Length(2, 200).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

        RuleFor(c => c.City)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido.")
            .Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

        RuleFor(c => c.State)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido.")
            .Length(2, 50).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

        RuleFor(c => c.Number)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido.")
            .Length(2, 50).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");
    }
}