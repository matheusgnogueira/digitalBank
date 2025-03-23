using DigitalBank.Application.DTOs.Transferencia;

namespace DigitalBank.Application.Interfaces;

public interface ITransferenciaService
{
    Task<TransferenciaRetornoDTO> RealizarTransferenciaAsync(TransferenciaDTO transferenciaDTO);
}
