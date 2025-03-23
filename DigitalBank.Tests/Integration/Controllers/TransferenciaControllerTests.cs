using DigitalBank.Application.DTOs.Transferencia;
using DigitalBank.Tests.Helpers;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace DigitalBank.Tests.Integration.Controllers;

public class TransferenciaControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TransferenciaControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task POST_RealizarTransferencia_DeveRetornar200OK_QuandoValido()
    {
        // Arrange: cria duas contas antes de transferir
        var contaOrigem = new { nome = "Conta Origem", documento = Guid.NewGuid().ToString().Substring(0, 20) };
        var contaDestino = new { nome = "Conta Destino", documento = Guid.NewGuid().ToString().Substring(0, 20) };

        var origemResponse = await _client.PostAsJsonAsync("/api/conta", contaOrigem);
        var destinoResponse = await _client.PostAsJsonAsync("/api/conta", contaDestino);

        var origemResult = await origemResponse.Content.ReadFromJsonAsync<ContaResponse>();
        var destinoResult = await destinoResponse.Content.ReadFromJsonAsync<ContaResponse>();

        var dto = new TransferenciaDTO(
            Guid.Parse(origemResult!.Data!.Id!.ToString()!),
            Guid.Parse(destinoResult!.Data!.Id!.ToString()!),
            200
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/transferencia", dto);
        var result = await response.Content.ReadFromJsonAsync<ResponseModel>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
        result.Message.Should().Be("Transferência realizada com sucesso");
    }

    [Fact(DisplayName = "Deve retornar 400 ao transferir de conta inexistente")]
    public async Task POST_Transferencia_DeveRetornar400_QuandoContaOrigemInexistente()
    {
        // Arrange: cria só a conta destino
        var contaDestino = new { nome = "Conta Destino", documento = Guid.NewGuid().ToString().Substring(0, 20) };
        var destinoResponse = await _client.PostAsJsonAsync("/api/conta", contaDestino);
        var destinoResult = await destinoResponse.Content.ReadFromJsonAsync<ContaResponse>();

        var dto = new TransferenciaDTO(Guid.NewGuid(), destinoResult!.Data!.Id, 100);

        // Act
        var response = await _client.PostAsJsonAsync("/api/transferencia", dto);
        var result = await response.Content.ReadFromJsonAsync<ResponseModel>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result!.Success.Should().BeFalse();
        result.Message.Should().Be("Conta de origem não encontrada.");
    }

    [Fact(DisplayName = "Deve retornar 400 ao transferir valor maior que o saldo")]
    public async Task POST_Transferencia_DeveRetornar400_QuandoSaldoInsuficiente()
    {
        // Arrange: cria as duas contas
        var origem = new { nome = "Origem", documento = Guid.NewGuid().ToString().Substring(0, 20) };
        var destino = new { nome = "Destino", documento = Guid.NewGuid().ToString().Substring(0, 20) };

        var origemResponse = await _client.PostAsJsonAsync("/api/conta", origem);
        var destinoResponse = await _client.PostAsJsonAsync("/api/conta", destino);

        var origemResult = await origemResponse.Content.ReadFromJsonAsync<ContaResponse>();
        var destinoResult = await destinoResponse.Content.ReadFromJsonAsync<ContaResponse>();

        var dto = new TransferenciaDTO(origemResult!.Data!.Id, destinoResult!.Data!.Id, 2000); // maior que saldo

        // Act
        var response = await _client.PostAsJsonAsync("/api/transferencia", dto);
        var result = await response.Content.ReadFromJsonAsync<ResponseModel>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result!.Success.Should().BeFalse();
        result.Message.Should().Be("Saldo insuficiente para realizar a transferência.");
    }

    private class ContaResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ContaData? Data { get; set; }
    }

    private class ContaData
    {
        public Guid Id { get; set; }
    }
}
