using Estoque.Application.DTOs;
using Estoque.Application.Mappings;
using Estoque.Domain.Entities;
using Estoque.Domain.Interfaces;
using Estoque.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Estoque.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoRepository _repository;
    private readonly EstoqueDbContext _dbContext; // Utilizando direto p/ controle da tabela de idempotencia na mesma transaçao EF
    
    public ProdutosController(IProdutoRepository repository, EstoqueDbContext dbContext)
    {
        _repository = repository;
        _dbContext = dbContext;
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
    public async Task<IActionResult> AbaterSaldo([FromBody] List<BaixaEstoqueDTO> itens, [FromHeader(Name = "X-Idempotency-Key")] Guid? idempotencyKey)
    {
        if (itens is null || !itens.Any()) return BadRequest("Nenhum item enviado para baixa de estoque.");

        // Implementação de Idempotência
        if (idempotencyKey.HasValue && idempotencyKey.Value != Guid.Empty)
        {
            var jaProcessada = await _dbContext.TransacoesProcessadas.AnyAsync(t => t.ChaveIdempotencia == idempotencyKey.Value);
            if (jaProcessada)
            {
                // Se já processou antes (retry), ignora a operação física mas devolve 200 pro cliente não falhar.
                return Ok(new { Mensagem = "Transação de abate de estoque já foi processada anteriormente. Efeito Neutro retornado.", IdempotencyKey = idempotencyKey.Value });
            }
        }

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
        
        // Registrar a chave de idempotência no contexto
        if (idempotencyKey.HasValue && idempotencyKey.Value != Guid.Empty)
        {
            await _dbContext.TransacoesProcessadas.AddAsync(new TransacaoProcessada(idempotencyKey.Value));
        }

        try
        {
            await _repository.SalvarAlteracoesAsync();
            return Ok(new { Mensagem = "Estoque atualizado com sucesso!" });
        }
        catch (DbUpdateConcurrencyException)
        {
            // Ocorreu uma colisão: dois processos tentaram atualizar o saldo ao mesmo tempo
            // Retornamos 409 Conflict para o Faturamento / Cliente poder tentar novamente (Retry Policy)
            return Conflict(new { Erro = "O saldo de um ou mais produtos foi alterado por outra transação simultânea. Tente novamente." });
        }
    }
}