using DigitalBank.Application.DTOs.Transferencia;
using DigitalBank.Application.Services;
using DigitalBank.Domain.Entities;
using DigitalBank.Domain.Interfaces;
using DigitalBank.Util.Exceptions;
using FluentAssertions;
using Moq;

namespace DigitalBank.Tests.Services
{
    public class TransferenciaServiceTests
    {
        private readonly Mock<IContaRepository> _contaRepositoryMock;
        private readonly Mock<ITransferenciaRepository> _transferenciaRepositoryMock;
        private readonly TransferenciaService _transferenciaService;

        public TransferenciaServiceTests()
        {
            _contaRepositoryMock = new Mock<IContaRepository>();
            _transferenciaRepositoryMock = new Mock<ITransferenciaRepository>();
            _transferenciaService = new TransferenciaService(_contaRepositoryMock.Object, _transferenciaRepositoryMock.Object);
        }

        [Fact(DisplayName = "Não deve permitir transferir entre a mesma conta")]
        public async Task Transferir_MesmaConta_DeveLancarExcecao()
        {
            // Arrange
            var dto = new TransferenciaDTO(Guid.NewGuid(), Guid.NewGuid(), 100);
            var mesmoId = dto.ContaOrigemId;
            dto = dto with { ContaDestinoId = mesmoId };

            // Act
            var act = async () => await _transferenciaService.RealizarTransferenciaAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("A conta de origem e destino não podem ser a mesma.");
        }

        [Fact(DisplayName = "Deve lançar exceção se conta de origem não for encontrada")]
        public async Task Transferir_ContaOrigemNaoEncontrada_DeveLancarExcecao()
        {
            // Arrange
            var dto = new TransferenciaDTO(Guid.NewGuid(), Guid.NewGuid(), 100);
            _contaRepositoryMock.Setup(x => x.ObterPorIdAsync(dto.ContaOrigemId)).ReturnsAsync((Conta?)null);

            // Act
            var act = async () => await _transferenciaService.RealizarTransferenciaAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("Conta de origem não encontrada.");
        }

        [Fact(DisplayName = "Deve lançar exceção se conta de destino não for encontrada")]
        public async Task Transferir_ContaDestinoNaoEncontrada_DeveLancarExcecao()
        {
            // Arrange
            var contaOrigem = new Conta("Origem", "111");
            var dto = new TransferenciaDTO(contaOrigem.Id, Guid.NewGuid(), 100);

            _contaRepositoryMock.Setup(x => x.ObterPorIdAsync(dto.ContaOrigemId)).ReturnsAsync(contaOrigem);
            _contaRepositoryMock.Setup(x => x.ObterPorIdAsync(dto.ContaDestinoId)).ReturnsAsync((Conta?)null);

            // Act
            var act = async () => await _transferenciaService.RealizarTransferenciaAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("Conta de destino não encontrada.");
        }

        [Fact(DisplayName = "Deve transferir com sucesso se tudo estiver correto")]
        public async Task Transferir_DeveExecutarComSucesso()
        {
            // Arrange
            var contaOrigem = new Conta("Origem", "111");
            var contaDestino = new Conta("Destino", "222");
            var dto = new TransferenciaDTO(contaOrigem.Id, contaDestino.Id, 100);

            _contaRepositoryMock.Setup(x => x.ObterPorIdAsync(contaOrigem.Id)).ReturnsAsync(contaOrigem);
            _contaRepositoryMock.Setup(x => x.ObterPorIdAsync(contaDestino.Id)).ReturnsAsync(contaDestino);

            // Act
            var result = await _transferenciaService.RealizarTransferenciaAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Valor.Should().Be(100);
            result.ContaOrigemId.Should().Be(contaOrigem.Id);
            result.ContaDestinoId.Should().Be(contaDestino.Id);

            _contaRepositoryMock.Verify(x => x.AtualizarAsync(contaOrigem), Times.Once);
            _contaRepositoryMock.Verify(x => x.AtualizarAsync(contaDestino), Times.Once);
            _transferenciaRepositoryMock.Verify(x => x.InserirAsync(It.IsAny<Transferencia>()), Times.Once);
        }
    }
}
