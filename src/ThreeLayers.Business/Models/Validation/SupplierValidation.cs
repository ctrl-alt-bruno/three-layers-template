using FluentValidation;
using ThreeLayers.Business.Models.Validation.Documents;

namespace ThreeLayers.Business.Models.Validation;

public class SupplierValidation : AbstractValidator<Supplier>
{
    public SupplierValidation()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The field {PropertyName} is required.")
            .Length(2, 100).WithMessage("The field {PropertyName} must be between {MinLength} and {MaxLength} characters long.");

        RuleFor(c => c.Document)
            .NotEmpty().WithMessage("The field {PropertyName} is required.")
            .Must(BeValidDocument).WithMessage("The document is not valid.");

        RuleFor(c => c.SupplierType)
            .IsInEnum().WithMessage("The field {PropertyName} must be a valid supplier type.");

        When(x => x.SupplierType == SupplierTypes.Regular, () =>
        {
            RuleFor(x => x.Document)
                .Length(11).WithMessage("CPF must have exactly {ExpectedLength} characters.")
                .Must(DocumentsValidator.ValidateCpf).WithMessage("CPF is not valid.");
        });

        When(x => x.SupplierType == SupplierTypes.Legal, () =>
        {
            RuleFor(x => x.Document)
                .Length(14).WithMessage("CNPJ must have exactly {ExpectedLength} characters.")
                .Must(DocumentsValidator.ValidateCnpj).WithMessage("CNPJ is not valid.");
        });
    }

    private bool BeValidDocument(Supplier supplier, string document)
    {
        return supplier.SupplierType switch
        {
            SupplierTypes.Regular => DocumentsValidator.ValidateCpf(document),
            SupplierTypes.Legal => DocumentsValidator.ValidateCnpj(document),
            _ => false
        };
    }
}