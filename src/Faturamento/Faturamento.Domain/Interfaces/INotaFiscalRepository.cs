using Faturamento.Domain.Entities;

namespace Faturamento.Domain.Interfaces;

public interface INotaFiscalRepository
{
    Task AdicionarAsync(NotaFiscal notaFiscal);
    Task<NotaFiscal?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<NotaFiscal>> ObterTodasAsync();
    Task<int> ObterProximoNumeroSequencialAsync();
    Task SalvarAlteracoesAsync();
}