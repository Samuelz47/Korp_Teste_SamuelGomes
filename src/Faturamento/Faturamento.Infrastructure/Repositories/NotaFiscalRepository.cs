using Faturamento.Domain.Entities;
using Faturamento.Domain.Interfaces;
using Faturamento.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Faturamento.Infrastructure.Repositories;

public class NotaFiscalRepository : INotaFiscalRepository
{
    private readonly FaturamentoDbContext _context;

    public NotaFiscalRepository(FaturamentoDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(NotaFiscal notaFiscal)
    {
        await _context.NotasFiscais.AddAsync(notaFiscal);
    }

    public async Task<NotaFiscal?> ObterPorIdAsync(Guid id)
    {
        return await _context.NotasFiscais.Include((n => n.Itens)).FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<IEnumerable<NotaFiscal>> ObterTodasAsync()
    {
        return await _context.NotasFiscais.Include(n => n.Itens).ToListAsync();
    }

    public async Task<int> ObterProximoNumeroSequencialAsync()
    {
        var max = await _context.NotasFiscais.MaxAsync(n => (int?)n.NumeroSequencial);
        return (max ?? 0) + 1; // Começa de 1 se estiver vazio, incrementa em +1 os existentes
    }

    public async Task SalvarAlteracoesAsync()
    {
        await _context.SaveChangesAsync();
    }
}