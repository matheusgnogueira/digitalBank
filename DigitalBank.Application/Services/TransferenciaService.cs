using DigitalBank.Application.DTOs.Transferencia;
using DigitalBank.Application.Interfaces;
using DigitalBank.Domain.Entities;
using DigitalBank.Domain.Interfaces;
using DigitalBank.Util.Exceptions;

namespace DigitalBank.Application.Services;

public class TransferenciaService : ITransferenciaService
{
    private readonly IContaRepository _contaRepository;
    private readonly ITransferenciaRepository _transferenciaRepository;

    public TransferenciaService(
        IContaRepository contaRepository,
        ITransferenciaRepository transferenciaRepository)
    {
        _contaRepository = contaRepository;
        _transferenciaRepository = transferenciaRepository;
    }

    public async Task<TransferenciaRetornoDTO> RealizarTransferenciaAsync(TransferenciaDTO transferenciaDTO)
    {
        if (transferenciaDTO.ContaOrigemId == transferenciaDTO.ContaDestinoId)
            throw new DomainException("A conta de origem e destino não podem ser a mesma.");

        var contaOrigem = await _contaRepository.ObterPorIdAsync(transferenciaDTO.ContaOrigemId)
            ?? throw new DomainException("Conta de origem não encontrada.");

        var contaDestino = await _contaRepository.ObterPorIdAsync(transferenciaDTO.ContaDestinoId)
            ?? throw new DomainException("Conta de destino não encontrada.");

        contaOrigem.Debitar(transferenciaDTO.Valor);
        contaDestino.Creditar(transferenciaDTO.Valor);

        var transferencia = new Transferencia(
            contaOrigem.Id,
            contaDestino.Id,
            transferenciaDTO.Valor
        );

        await _contaRepository.AtualizarAsync(contaOrigem);
        await _contaRepository.AtualizarAsync(contaDestino);
        await _transferenciaRepository.InserirAsync(transferencia);

        return new TransferenciaRetornoDTO
        {
            Id = transferencia.Id,
            ContaOrigemId = transferencia.ContaOrigemId,
            ContaDestinoId = transferencia.ContaDestinoId,
            Valor = transferencia.Valor,
            DataTransferencia = transferencia.DataTransferencia
        };
    }
}
