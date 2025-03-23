using DigitalBank.Application.DTOs.Conta;
using DigitalBank.Tests.Helpers;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace DigitalBank.Tests.Integration.Controllers;

public class ContaControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ContaControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task POST_CriarConta_DeveRetornar201Created_QuandoValido()
    {
        // Arrange
        var novaConta = new ContaCriacaoDTO("Carlos Silva", "9988776655");

        // Act
        var response = await _client.PostAsJsonAsync("/api/conta", novaConta);
        var result = await response.Content.ReadFromJsonAsync<ResponseModel>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
        result.Message.Should().Be("Conta criada com sucesso");
    }

    [Fact(DisplayName = "Deve retornar 400 ao tentar criar conta com documento já existente")]
    public async Task POST_CriarConta_DeveRetornar400_QuandoDocumentoExistente()
    {
        // Arrange
        var dto = new ContaCriacaoDTO("Matheus", "12345678900");

        // Act - cria a primeira conta com sucesso
        await _client.PostAsJsonAsync("/api/conta", dto);

        // Act - tenta criar novamente com o mesmo documento
        var response = await _client.PostAsJsonAsync("/api/conta", dto);
        var result = await response.Content.ReadFromJsonAsync<ResponseModel>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result!.Success.Should().BeFalse();
        result.Message.Should().Be("Já existe uma conta com esse documento.");
    }

    [Fact(DisplayName = "Deve retornar 400 ao tentar inativar conta inexistente")]
    public async Task PUT_InativarConta_DeveRetornar400_QuandoContaNaoEncontrada()
    {
        // Arrange
        var dto = new ContaInativacaoDTO("99999999999", "Sistema");

        // Act
        var response = await _client.PutAsJsonAsync("/api/conta/inativar", dto);
        var result = await response.Content.ReadFromJsonAsync<ResponseModel>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result!.Success.Should().BeFalse();
        result.Message.Should().Be("Conta não encontrada.");
    }

    [Fact(DisplayName = "Deve retornar 404 ao buscar conta com documento inexistente")]
    public async Task GET_ObterConta_DeveRetornar404_QuandoDocumentoNaoEncontrado()
    {
        var response = await _client.GetAsync("/api/conta/99999999999");
        var result = await response.Content.ReadFromJsonAsync<ResponseModel>();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result!.Success.Should().BeFalse();
        result.Message.Should().Be("Conta não encontrada");
    }
}
