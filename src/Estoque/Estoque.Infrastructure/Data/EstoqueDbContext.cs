using Estoque.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Estoque.Infrastructure.Data;

public class EstoqueDbContext : DbContext
{
    public EstoqueDbContext(DbContextOptions<EstoqueDbContext> options) : base(options) { }
    
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<TransacaoProcessada> TransacoesProcessadas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produto>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Codigo).IsRequired().HasMaxLength(50);
            e.Property(p => p.Descricao).IsRequired().HasMaxLength(200);
            e.Property(p => p.Saldo).IsRequired();
            e.Property(p => p.Versao).IsConcurrencyToken();
        });

        modelBuilder.Entity<TransacaoProcessada>(e =>
        {
            e.HasKey(t => t.Id);
            e.HasIndex(t => t.ChaveIdempotencia).IsUnique();
        });
        
        base.OnModelCreating(modelBuilder);
    }
}