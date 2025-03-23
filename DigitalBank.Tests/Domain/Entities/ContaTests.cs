using DigitalBank.Domain.Entities;
using DigitalBank.Util.Enums;
using DigitalBank.Util.Exceptions;
using FluentAssertions;

namespace DigitalBank.Tests.Domain.Entities;

public class ContaTests
{
    [Fact(DisplayName = "Deve criar conta com dados válidos")]
    public void CriarConta_Valida_DeveCriarComSaldoInicialEStatusAtiva()
    {
        // Arrange
        var nome = "Matheus Garcia";
        var documento = "12345678900";

        // Act
        var conta = new Conta(nome, documento);

        // Assert
        conta.Should().NotBeNull();
        conta.Nome.Should().Be(nome);
        conta.Documento.Should().Be(documento);
        conta.Saldo.Should().Be(1000);
        conta.Status.Should().Be(StatusConta.Ativa);
        conta.DataAbertura.Date.Should().Be(DateTime.Now.Date);
        conta.DataInativacao.Should().BeNull();
        conta.UsuarioInativacao.Should().BeNull();
    }

    [Fact(DisplayName = "Não deve criar conta com nome vazio")]
    public void CriarConta_NomeVazio_DeveLancarExcecao()
    {
        var act = () => new Conta("", "123");

        act.Should().Throw<DomainException>()
            .WithMessage("Nome é obrigatório.");
    }

    [Fact(DisplayName = "Não deve criar conta com documento vazio")]
    public void CriarConta_DocumentoVazio_DeveLancarExcecao()
    {
        var act = () => new Conta("Matheus", "");

        act.Should().Throw<DomainException>()
            .WithMessage("Documento é obrigatório.");
    }

    [Fact(DisplayName = "Deve inativar conta válida")]
    public void InativarConta_Ativa_DeveAlterarStatusERegistrarDados()
    {
        // Arrange
        var conta = new Conta("Matheus", "123");
        var usuario = "Admin";

        // Act
        conta.Inativar(usuario);

        // Assert
        conta.Status.Should().Be(StatusConta.Inativa);
        conta.DataInativacao.Should().NotBeNull();
        conta.UsuarioInativacao.Should().Be(usuario);
    }

    [Fact(DisplayName = "Não deve inativar conta já inativa")]
    public void InativarConta_JaInativa_DeveLancarExcecao()
    {
        var conta = new Conta("Matheus", "123");
        conta.Inativar("Admin");

        var act = () => conta.Inativar("Outro");

        act.Should().Throw<DomainException>()
            .WithMessage("Conta já está inativa.");
    }

    [Fact(DisplayName = "Deve debitar de conta ativa com saldo suficiente")]
    public void DebitarConta_SaldoSuficiente_DeveReduzirSaldo()
    {
        var conta = new Conta("Matheus", "123");

        conta.Debitar(500);

        conta.Saldo.Should().Be(500);
    }

    [Fact(DisplayName = "Não deve debitar de conta inativa")]
    public void DebitarConta_Inativa_DeveLancarExcecao()
    {
        var conta = new Conta("Matheus", "123");
        conta.Inativar("Admin");

        var act = () => conta.Debitar(100);

        act.Should().Throw<DomainException>()
            .WithMessage("A conta está inativa. Não é possível realizar a transferência.");
    }

    [Fact(DisplayName = "Não deve debitar se saldo for insuficiente")]
    public void DebitarConta_SaldoInsuficiente_DeveLancarExcecao()
    {
        var conta = new Conta("Matheus", "123");

        var act = () => conta.Debitar(2000);

        act.Should().Throw<DomainException>()
            .WithMessage("Saldo insuficiente para realizar a transferência.");
    }

    [Fact(DisplayName = "Deve creditar valor em conta ativa")]
    public void CreditarConta_Ativa_DeveAumentarSaldo()
    {
        var conta = new Conta("Matheus", "123");

        conta.Creditar(500);

        conta.Saldo.Should().Be(1500);
    }

    [Fact(DisplayName = "Não deve creditar em conta inativa")]
    public void CreditarConta_Inativa_DeveLancarExcecao()
    {
        var conta = new Conta("Matheus", "123");
        conta.Inativar("Admin");

        var act = () => conta.Creditar(100);

        act.Should().Throw<DomainException>()
            .WithMessage("A conta está inativa. Não é possível receber transferências.");
    }
}
