using DigitalBank.Domain.Entities;
using DigitalBank.Util.Exceptions;
using FluentAssertions;

namespace DigitalBank.Tests.Domain.Entities;

public class TransferenciaTests
{
    [Fact(DisplayName = "Deve criar transferência válida quando dados corretos")]
    public void Construtor_DeveCriarTransferencia_QuandoDadosValidos()
    {
        // Arrange
        var contaOrigemId = Guid.NewGuid();
        var contaDestinoId = Guid.NewGuid();
        var valor = 100;

        // Act
        var transferencia = new Transferencia(contaOrigemId, contaDestinoId, valor);

        // Assert
        transferencia.ContaOrigemId.Should().Be(contaOrigemId);
        transferencia.ContaDestinoId.Should().Be(contaDestinoId);
        transferencia.Valor.Should().Be(valor);
        transferencia.DataTransferencia.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        transferencia.Id.Should().NotBeEmpty();
    }

    [Theory(DisplayName = "Deve lançar exceção quando valor da transferência for inválido")]
    [InlineData(0)]
    [InlineData(-10)]
    public void Construtor_DeveLancarExcecao_QuandoValorMenorOuIgualZero(decimal valorInvalido)
    {
        // Arrange
        var contaOrigemId = Guid.NewGuid();
        var contaDestinoId = Guid.NewGuid();

        // Act
        Action act = () => new Transferencia(contaOrigemId, contaDestinoId, valorInvalido);

        // Assert
        act.Should()
            .Throw<DomainException>()
            .WithMessage("Valor da transferência deve ser maior que zero.");
    }
}
