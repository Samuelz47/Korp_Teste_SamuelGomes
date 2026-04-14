using Faturamento.Domain.Entities;

namespace Faturamento.Domain.Interfaces;

public interface INotaFiscalRepository
{
    Task AdicionarAsync(NotaFiscal notaFiscal);
    Task<NotaFiscal?> ObterPorIdAsync(Guid id);
    Task SalvarAlteracoesAsync();
}