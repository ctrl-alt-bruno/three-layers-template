using FluentValidation;

namespace ThreeLayers.Business.Models.Validation;

public class SupplierValidation : AbstractValidator<Supplier>
{
    public SupplierValidation()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido.")
            .Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

        When(x => x.SupplierType == SupplierTypes.Regular, () =>
        {
            RuleFor(x => x.Document.Length).Equal(11)
                .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");
        });

        When(x => x.SupplierType == SupplierTypes.Legal, () =>
        {

        });
    }
}