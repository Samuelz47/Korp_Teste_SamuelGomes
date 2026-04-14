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
        var sucessoEstoque = await _estoqueClient.AbaterSaldoAsync(notaFiscalDto.Itens);
        
        if (!sucessoEstoque)
        {
            throw new InvalidOperationException("Não foi possível aprovar a Nota Fiscal. Verifique o saldo no Estoque ou a disponibilidade do sistema.");
        }

        var notaFiscal = new NotaFiscal();

        foreach (var item in notaFiscalDto.Itens)
        {
            notaFiscal.AdicionarItem(item.ProdutoCodigo, item.Quantidade);
        }
        
        await _repository.AdicionarAsync(notaFiscal);
        await _repository.SalvarAlteracoesAsync();
        return notaFiscal;
    }
}