using Estoque.Domain.Entities;
using Estoque.Domain.Interfaces;
using Estoque.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Estoque.Infrastructure.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly EstoqueDbContext _context;

    public ProdutoRepository(EstoqueDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produto>> ObterTodosAsync()
    {
        return await _context.Produtos.AsNoTracking().ToListAsync();
    }

    public async Task<Produto?> ObterPorCodigoAsync(string codigo)
    {
        return await _context.Produtos.FirstOrDefaultAsync(p => p.Codigo == codigo);
    }

    public async Task AdicionarAsync(Produto produto)
    {
        await _context.Produtos.AddAsync(produto);
    }

    public async Task SalvarAlteracoesAsync()
    {
        await _context.SaveChangesAsync();
    }
}