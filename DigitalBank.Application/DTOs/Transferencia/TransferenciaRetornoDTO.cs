namespace DigitalBank.Application.DTOs.Transferencia;

public record TransferenciaRetornoDTO
{
    public Guid Id { get; init; }
    public Guid ContaOrigemId { get; init; }
    public Guid ContaDestinoId { get; init; }
    public decimal Valor { get; init; }
    public DateTime DataTransferencia { get; init; }
}
