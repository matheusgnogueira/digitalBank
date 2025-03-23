using AutoMapper;
using DigitalBank.Application.DTOs.Conta;
using DigitalBank.Application.Interfaces;
using DigitalBank.Domain.Entities;
using DigitalBank.Domain.Interfaces;
using DigitalBank.Util.Exceptions;

namespace DigitalBank.Application.Services;

public class ContaService : IContaService
{
    private readonly IContaRepository _contaRepository;
    private readonly IMapper _mapper;

    public ContaService(IContaRepository contaRepository, IMapper mapper)
    {
        _contaRepository = contaRepository;
        _mapper = mapper;
    }

    public async Task<ContaRetornoDTO> CriarContaAsync(ContaCriacaoDTO contaCriacaoDTO)
    {
        var existente = await _contaRepository.ObterPorDocumentoAsync(contaCriacaoDTO.Documento);
        if (existente != null)
            throw new DomainException("Já existe uma conta com esse documento.");

        var conta = new Conta(contaCriacaoDTO.Nome, contaCriacaoDTO.Documento);
        await _contaRepository.InserirAsync(conta);

        return _mapper.Map<ContaRetornoDTO>(conta);
    }

    public async Task<IEnumerable<ContaRetornoDTO>> BuscarContasAsync(string? nome, string? documento)
    {
        var contas = await _contaRepository.BuscarAsync(nome, documento);
        return contas.Select(c => _mapper.Map<ContaRetornoDTO>(c));
    }

    public async Task<ContaRetornoDTO> InativarContaAsync(ContaInativacaoDTO dto)
    {
        var conta = await _contaRepository.ObterPorDocumentoAsync(dto.Documento)
            ?? throw new DomainException("Conta não encontrada.");

        conta.Inativar(dto.UsuarioResponsavel);
        await _contaRepository.AtualizarAsync(conta);

        return _mapper.Map<ContaRetornoDTO>(conta);
    }

}
