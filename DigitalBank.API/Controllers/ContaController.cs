using DigitalBank.Application.DTOs.Conta;
using DigitalBank.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DigitalBank.API.Utilities;

namespace DigitalBank.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContaController : ControllerBase
{
    private readonly IContaService _contaService;

    public ContaController(IContaService contaService)
    {
        _contaService = contaService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CriarConta([FromBody] ContaCriacaoDTO dto)
    {
        var result = await _contaService.CriarContaAsync(dto);
        return CreatedAtAction(nameof(ObterPorDocumento), new { documento = result.Documento },
            new ResultViewModel(true, "Conta criada com sucesso", result));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarContas([FromQuery] string? nome, [FromQuery] string? documento)
    {
        var contas = await _contaService.BuscarContasAsync(nome, documento);
        return Ok(new ResultViewModel(true, "Contas listadas com sucesso", contas));
    }

    [HttpPut("inativar")]
    [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> InativarConta([FromBody] ContaInativacaoDTO dto)
    {
        var contaAtualizada = await _contaService.InativarContaAsync(dto);
        return Ok(new ResultViewModel(true, "Conta inativada com sucesso", contaAtualizada));
    }

    [HttpGet("{documento}")]
    [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorDocumento(string documento)
    {
        var contas = await _contaService.BuscarContasAsync(null, documento);
        var conta = contas.FirstOrDefault();

        return conta is null
            ? NotFound(new ResultViewModel(false, "Conta não encontrada"))
            : Ok(new ResultViewModel(true, "Conta encontrada com sucesso", conta));
    }
}
