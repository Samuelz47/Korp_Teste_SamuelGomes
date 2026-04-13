using Estoque.Application.DTOs;
using Estoque.Domain.Entities;

namespace Estoque.Application.Mappings;

public static class ProdutoMapper
{
    public static ProdutoDTO ToDto(this Produto produto)
    {
        if (produto == null) return null;

        return new ProdutoDTO
        {
            Codigo = produto.Codigo,
            Descricao = produto.Descricao,
            Saldo = produto.Saldo
        };
    }
}