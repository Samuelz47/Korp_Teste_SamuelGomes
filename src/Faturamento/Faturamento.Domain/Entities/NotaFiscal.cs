using Faturamento.Domain.Enums;

namespace Faturamento.Domain.Entities;

public class NotaFiscal
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public int NumeroSequencial { get; private set; }
    public Status Status { get; private set; }
    
    private readonly List<NotaFiscalItem> _itens = new();
    public IReadOnlyCollection<NotaFiscalItem> Itens => _itens.AsReadOnly();

    public NotaFiscal()
    {
        Status = Status.Aberta;
    }
    
    public void DefinirNumeroSequencial(int numero)
    {
        NumeroSequencial = numero;
    }
    
    public void AdicionarItem(string produtoCodigo, int quantidade)
    {
        _itens.Add(new NotaFiscalItem(produtoCodigo, quantidade));
    }

    public void FecharNota()
    {
        if (Status != Status.Aberta)
            throw new InvalidOperationException("Apenas notas com status 'Aberta' podem ser fechadas.");

        Status = Status.Fechada;
    }
}