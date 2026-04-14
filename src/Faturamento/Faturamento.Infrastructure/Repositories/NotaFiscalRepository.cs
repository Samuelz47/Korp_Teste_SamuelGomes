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

    public async Task SalvarAlteracoesAsync()
    {
        await _context.SaveChangesAsync();
    }
}