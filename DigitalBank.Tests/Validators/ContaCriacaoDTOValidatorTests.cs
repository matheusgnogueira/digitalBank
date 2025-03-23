using DigitalBank.API.Validators;
using DigitalBank.Application.DTOs.Conta;
using FluentValidation.TestHelper;

namespace DigitalBank.Tests.Validators;

public class ContaCriacaoDTOValidatorTests
{
    private readonly ContaCriacaoDTOValidator _validator = new();

    [Fact]
    public void Deve_Apresentar_Erro_Se_Nome_Estiver_Vazio()
    {
        var model = new ContaCriacaoDTO("", "123");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Nome);
    }

    [Fact]
    public void Deve_Apresentar_Erro_Se_Documento_Estiver_Vazio()
    {
        var model = new ContaCriacaoDTO("Matheus", "");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Documento);
    }

    [Fact]
    public void Deve_Passar_Se_Dados_Forem_Validos()
    {
        var model = new ContaCriacaoDTO("Matheus Garcia", "12345678900");
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
