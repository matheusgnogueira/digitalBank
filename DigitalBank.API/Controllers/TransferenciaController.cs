using DigitalBank.API.Utilities;
using DigitalBank.Application.DTOs.Transferencia;
using DigitalBank.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBank.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransferenciaController : ControllerBase
{
    private readonly ITransferenciaService _transferenciaService;

    public TransferenciaController(ITransferenciaService transferenciaService)
    {
        _transferenciaService = transferenciaService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RealizarTransferencia([FromBody] TransferenciaDTO dto)
    {
        var transferencia = await _transferenciaService.RealizarTransferenciaAsync(dto);
        return Ok(new ResultViewModel(true, "Transferência realizada com sucesso", transferencia));
    }
}
