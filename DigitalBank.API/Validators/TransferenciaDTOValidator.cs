using DigitalBank.Application.DTOs.Transferencia;
using FluentValidation;

namespace DigitalBank.API.Validators;

public class TransferenciaDTOValidator : AbstractValidator<TransferenciaDTO>
{
    public TransferenciaDTOValidator()
    {
        RuleFor(x => x.ContaOrigemId)
            .NotEqual(Guid.Empty).WithMessage("Conta de origem é obrigatória.");

        RuleFor(x => x.ContaDestinoId)
            .NotEqual(Guid.Empty).WithMessage("Conta de destino é obrigatória.")
            .NotEqual(x => x.ContaOrigemId).WithMessage("Conta de destino deve ser diferente da conta de origem.");

        RuleFor(x => x.Valor)
            .GreaterThan(0).WithMessage("Valor da transferência deve ser maior que zero.");
    }
}
