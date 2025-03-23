using DigitalBank.Util.Enums;

namespace DigitalBank.Application.DTOs.Conta;

public record ContaRetornoDTO
{
    public Guid Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string Documento { get; init; } = string.Empty;
    public decimal Saldo { get; init; }
    public DateTime DataAbertura { get; init; }
    public StatusConta Status { get; init; }
    public string? StatusDescricao { get; init; }
    public DateTime? DataInativacao { get; init; }
    public string? UsuarioInativacao { get; init; }
}
