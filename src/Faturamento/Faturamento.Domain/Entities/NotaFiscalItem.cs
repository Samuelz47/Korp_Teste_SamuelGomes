namespace Faturamento.Domain.Entities;

public class NotaFiscalItem
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid NotaFiscalId { get; private set; }
    public string ProdutoCodigo { get; private set; }
    public int Quantidade { get; private set; }

    public NotaFiscalItem(string produtoCodigo, int quantidade)
    {
        ProdutoCodigo = produtoCodigo;
        Quantidade = quantidade;
    }
}