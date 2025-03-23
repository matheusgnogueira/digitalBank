using AutoMapper;
using DigitalBank.Application.DTOs.Conta;
using DigitalBank.Application.Services;
using DigitalBank.Domain.Entities;
using DigitalBank.Domain.Interfaces;
using DigitalBank.Util.Enums;
using DigitalBank.Util.Exceptions;
using FluentAssertions;
using Moq;

namespace DigitalBank.Tests.Services;

public class ContaServiceTests
{
    private readonly Mock<IContaRepository> _contaRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ContaService _service;

    public ContaServiceTests()
    {
        _contaRepoMock = new Mock<IContaRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new ContaService(_contaRepoMock.Object, _mapperMock.Object);
    }

    [Fact(DisplayName = "Deve criar uma nova conta com sucesso")]
    public async Task CriarContaAsync_DeveCriarComSucesso()
    {
        // Arrange
        var dto = new ContaCriacaoDTO("Matheus", "12345678900");
        var conta = new Conta(dto.Nome, dto.Documento);
        var retornoDto = new ContaRetornoDTO
        {
            Id = conta.Id,
            Nome = dto.Nome,
            Documento = dto.Documento,
            Saldo = 1000,
            DataAbertura = conta.DataAbertura,
            Status = conta.Status
        };

        _contaRepoMock.Setup(r => r.ObterPorDocumentoAsync(dto.Documento)).ReturnsAsync((Conta?)null);
        _mapperMock.Setup(m => m.Map<ContaRetornoDTO>(It.IsAny<Conta>())).Returns(retornoDto);

        // Act
        var result = await _service.CriarContaAsync(dto);

        // Assert
        result.Nome.Should().Be(dto.Nome);
        result.Documento.Should().Be(dto.Documento);
        result.Saldo.Should().Be(1000);
        result.Status.Should().Be(StatusConta.Ativa);
    }

    [Fact(DisplayName = "Não deve criar conta com documento já existente")]
    public async Task CriarContaAsync_DeveLancarExcecao_SeContaExistente()
    {
        // Arrange
        var dto = new ContaCriacaoDTO("Matheus", "12345678900");
        var contaExistente = new Conta("Matheus", "12345678900");

        _contaRepoMock.Setup(r => r.ObterPorDocumentoAsync(dto.Documento)).ReturnsAsync(contaExistente);

        // Act
        Func<Task> act = async () => await _service.CriarContaAsync(dto);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("Já existe uma conta com esse documento.");
    }

    [Fact(DisplayName = "Deve buscar contas corretamente")]
    public async Task BuscarContasAsync_DeveRetornarLista()
    {
        // Arrange
        var contas = new List<Conta>
        {
            new Conta("Matheus", "123"),
            new Conta("João", "456")
        };

        var contasDto = contas.Select(c => new ContaRetornoDTO
        {
            Nome = c.Nome,
            Documento = c.Documento,
            Saldo = c.Saldo,
            Status = c.Status,
            DataAbertura = c.DataAbertura
        });

        _contaRepoMock.Setup(r => r.BuscarAsync(null, null)).ReturnsAsync(contas);
        _mapperMock.Setup(m => m.Map<ContaRetornoDTO>(It.IsAny<Conta>()))
                   .Returns((Conta conta) => contasDto.First(dto => dto.Documento == conta.Documento));

        // Act
        var result = await _service.BuscarContasAsync(null, null);

        // Assert
        result.Should().HaveCount(2);
        result.First().Nome.Should().Be("Matheus");
    }

    [Fact(DisplayName = "Deve inativar conta com sucesso")]
    public async Task InativarContaAsync_DeveInativarComSucesso()
    {
        // Arrange
        var conta = new Conta("Matheus", "123456");
        var dto = new ContaInativacaoDTO(conta.Documento, "Admin");

        _contaRepoMock.Setup(r => r.ObterPorDocumentoAsync(dto.Documento)).ReturnsAsync(conta);
        _mapperMock.Setup(m => m.Map<ContaRetornoDTO>(It.IsAny<Conta>()))
            .Returns(new ContaRetornoDTO
            {
                Nome = conta.Nome,
                Documento = conta.Documento,
                Status = StatusConta.Inativa,
                UsuarioInativacao = dto.UsuarioResponsavel
            });

        // Act
        var result = await _service.InativarContaAsync(dto);

        // Assert
        result.Status.Should().Be(StatusConta.Inativa);
        result.UsuarioInativacao.Should().Be("Admin");
    }

    [Fact(DisplayName = "Deve lançar exceção se conta não for encontrada na inativação")]
    public async Task InativarContaAsync_DeveLancarExcecao_ContaNaoEncontrada()
    {
        // Arrange
        var dto = new ContaInativacaoDTO("123", "Admin");
        _contaRepoMock.Setup(r => r.ObterPorDocumentoAsync(dto.Documento)).ReturnsAsync((Conta?)null);

        // Act
        Func<Task> act = async () => await _service.InativarContaAsync(dto);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("Conta não encontrada.");
    }
}
