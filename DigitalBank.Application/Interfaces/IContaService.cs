using DigitalBank.Application.DTOs.Conta;

namespace DigitalBank.Application.Interfaces;

public interface IContaService
{
    Task<ContaRetornoDTO> CriarContaAsync(ContaCriacaoDTO contaCriacaoDTO);
    Task<IEnumerable<ContaRetornoDTO>> BuscarContasAsync(string? nome, string? documento);
    Task<ContaRetornoDTO> InativarContaAsync(ContaInativacaoDTO contaInativacaoDTO);
}
