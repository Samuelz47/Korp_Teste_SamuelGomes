using System.ComponentModel.DataAnnotations;

namespace Estoque.Domain.Entities;

public class Produto
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Codigo { get; private set; }
    public string Descricao { get; private set; }
    public int Saldo { get; private set; }

    public Produto(int saldo, string descricao, string codigo)
    {
        Saldo = saldo;
        Descricao = descricao;
        Codigo = codigo;
    }
    
    public void AbaterSaldo(int quantidade)
    {
        if (Saldo < quantidade)
            throw new InvalidOperationException("Saldo insuficiente.");
        
        Saldo -= quantidade;
    }
}