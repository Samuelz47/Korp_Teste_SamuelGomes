using Faturamento.Application.DTOs;
using Faturamento.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Faturamento.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotasFiscaisController : ControllerBase
{
    private readonly INotaFiscalService _service;

    public NotasFiscaisController(INotaFiscalService service)
    {
        _service = service;
    }
    
    [HttpPost]
    public async Task<IActionResult> GerarNotaFiscal([FromBody] NotaFiscalForRegistrationDTO notaFiscalDto)
    {
        if (notaFiscalDto is null || !notaFiscalDto.Itens.Any())
        {
            return BadRequest("Nenhum item enviado para geração da Nota Fiscal.");
        }

        try
        {
            var notaFiscal = await _service.GerarNotaFiscalAsync(notaFiscalDto);
            return Ok(new 
            { 
                Mensagem = "Nota Fiscal gerada com sucesso!", 
                NotaId = notaFiscal.Id 
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Erro = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Erro = "Ocorreu um erro interno ao processar a requisição", Detalhe = ex.Message });
        }
    }
}