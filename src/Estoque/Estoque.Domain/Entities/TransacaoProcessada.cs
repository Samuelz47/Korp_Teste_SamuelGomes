using System;

namespace Estoque.Domain.Entities;

public class TransacaoProcessada
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid ChaveIdempotencia { get; private set; }
    public DateTime DataProcessamento { get; private set; } = DateTime.UtcNow;

    public TransacaoProcessada(Guid chaveIdempotencia)
    {
        ChaveIdempotencia = chaveIdempotencia;
    }

    private TransacaoProcessada() { } // EF Core
}
