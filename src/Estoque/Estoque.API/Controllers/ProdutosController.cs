using Estoque.Application.DTOs;
using Estoque.Application.Mappings;
using Estoque.Domain.Entities;
using Estoque.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Estoque.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoRepository _repository;
    
    public ProdutosController(IProdutoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        var produtos = await _repository.ObterTodosAsync();
        var produtosDto = produtos.Select(p => p.ToDto());
        return Ok(produtosDto);
    }
    
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] ProdutoForRegistrationDTO dto)
    {
        var produtoExistente = await _repository.ObterPorCodigoAsync(dto.Codigo);
        if (produtoExistente != null)
        {
            return BadRequest("Já existe um produto cadastrado com este código.");
        }

        var produto = new Produto(dto.Saldo, dto.Descricao, dto.Codigo);
        
        await _repository.AdicionarAsync(produto);
        await _repository.SalvarAlteracoesAsync();

        return CreatedAtAction(nameof(ObterTodos), new { id = produto.Id }, produto);
    }

    [HttpPut]
    public async Task<IActionResult> AbaterSaldo([FromBody] List<BaixaEstoqueDTO> itens)
    {
        if (itens is null || !itens.Any()) return BadRequest("Nenhum item enviado para baixa de estoque.");

        foreach (var item in itens)
        {
            var produto = await _repository.ObterPorCodigoAsync(item.ProdutoCodigo);
            
            if (produto is null) return NotFound($"Produto com código {item.ProdutoCodigo} não encontrado.");

            try
            {
                produto.AbaterSaldo(item.Quantidade);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Erro = ex.Message, Produto = item.ProdutoCodigo });
            }
        }
        
        await _repository.SalvarAlteracoesAsync();
        return Ok(new { Mensagem = "Estoque atualizado com sucesso!" });
    }
}