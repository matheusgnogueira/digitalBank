using DigitalBank.Application.DTOs.Conta;
using FluentValidation;

namespace DigitalBank.API.Validators;

public class ContaCriacaoDTOValidator : AbstractValidator<ContaCriacaoDTO>
{
    public ContaCriacaoDTOValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Documento)
            .NotEmpty().WithMessage("Documento é obrigatório.")
            .MaximumLength(20).WithMessage("Documento deve ter no máximo 20 caracteres.");
    }
}
