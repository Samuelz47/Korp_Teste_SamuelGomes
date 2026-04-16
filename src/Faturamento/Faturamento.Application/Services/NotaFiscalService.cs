using Faturamento.Application.DTOs;
using Faturamento.Application.Interfaces;
using Faturamento.Domain.Entities;
using Faturamento.Domain.Interfaces;

namespace Faturamento.Application.Services;

public class NotaFiscalService : INotaFiscalService
{
    private readonly INotaFiscalRepository _repository;
    private readonly IEstoqueClient _estoqueClient;

    public NotaFiscalService(IEstoqueClient estoqueClient, INotaFiscalRepository repository)
    {
        _estoqueClient = estoqueClient;
        _repository = repository;
    }

    public async Task<NotaFiscal> GerarNotaFiscalAsync(NotaFiscalForRegistrationDTO notaFiscalDto)
    {
        var numero = await _repository.ObterProximoNumeroSequencialAsync();
        var notaFiscal = new NotaFiscal();
        notaFiscal.DefinirNumeroSequencial(numero);

        foreach (var item in notaFiscalDto.Itens)
        {
            notaFiscal.AdicionarItem(item.ProdutoCodigo, item.Quantidade);
        }
        
        await _repository.AdicionarAsync(notaFiscal);
        await _repository.SalvarAlteracoesAsync();
        return notaFiscal;
    }

    public async Task<IEnumerable<NotaFiscal>> ObterTodasNotasFiscaisAsync()
    {
        return await _repository.ObterTodasAsync();
    }

    public async Task FecharNotaFiscalAsync(Guid id)
    {
        var notaFiscal = await _repository.ObterPorIdAsync(id);
        if (notaFiscal == null)
            throw new InvalidOperationException("Nota Fiscal não encontrada.");

        var itensParaBaixa = notaFiscal.Itens
            .Select(i => new NotaFiscalItemDTO
            {
                ProdutoCodigo = i.ProdutoCodigo,
                Quantidade = i.Quantidade
            }).ToList();

        var sucessoEstoque = await _estoqueClient.AbaterSaldoAsync(notaFiscal.Id, itensParaBaixa);
        if (!sucessoEstoque)
            throw new InvalidOperationException("Não foi possível abater o estoque. Verifique o saldo ou a disponibilidade do serviço.");

        notaFiscal.FecharNota();
        await _repository.SalvarAlteracoesAsync();
    }
}