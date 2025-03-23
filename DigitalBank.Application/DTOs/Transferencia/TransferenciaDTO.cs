namespace DigitalBank.Application.DTOs.Transferencia;

public record TransferenciaDTO(Guid ContaOrigemId, Guid ContaDestinoId, decimal Valor);
