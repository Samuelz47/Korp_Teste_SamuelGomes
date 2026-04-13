using Estoque.Domain.Entities;

namespace Estoque.Domain.Interfaces;

public interface IProdutoRepository
{
    Task<IEnumerable<Produto>> ObterTodosAsync();
    Task<Produto?> ObterPorCodigoAsync(string codigo);
    Task AdicionarAsync(Produto produto);
    Task SalvarAlteracoesAsync();
}