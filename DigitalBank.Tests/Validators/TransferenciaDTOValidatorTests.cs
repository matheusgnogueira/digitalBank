using DigitalBank.API.Validators;
using DigitalBank.Application.DTOs.Transferencia;
using FluentValidation.TestHelper;

namespace DigitalBank.Tests.Validators;

public class TransferenciaDTOValidatorTests
{
    private readonly TransferenciaDTOValidator _validator = new();

    [Fact]
    public void Deve_Apresentar_Erro_Se_Conta_Origem_For_Vazia()
    {
        var dto = new TransferenciaDTO(Guid.Empty, Guid.NewGuid(), 100);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.ContaOrigemId);
    }

    [Fact]
    public void Deve_Apresentar_Erro_Se_Conta_Destino_For_Vazia()
    {
        var dto = new TransferenciaDTO(Guid.NewGuid(), Guid.Empty, 100);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.ContaDestinoId);
    }

    [Fact]
    public void Deve_Apresentar_Erro_Se_Contas_Forem_Iguais()
    {
        var id = Guid.NewGuid();
        var dto = new TransferenciaDTO(id, id, 100);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.ContaDestinoId);
    }

    [Fact]
    public void Deve_Apresentar_Erro_Se_Valor_For_Menor_Que_1()
    {
        var dto = new TransferenciaDTO(Guid.NewGuid(), Guid.NewGuid(), 0);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Valor);
    }

    [Fact]
    public void Deve_Passar_Se_Todos_Dados_Estiverem_Certos()
    {
        var dto = new TransferenciaDTO(Guid.NewGuid(), Guid.NewGuid(), 100);
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
