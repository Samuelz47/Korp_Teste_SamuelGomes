using Faturamento.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Faturamento.Infrastructure.Data;

public class FaturamentoDbContext : DbContext
{
    public FaturamentoDbContext(DbContextOptions<FaturamentoDbContext> options) : base(options) { }
    
    public DbSet<NotaFiscal> NotasFiscais { get; set; }
    public DbSet<NotaFiscalItem> NotaFiscalItems { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotaFiscal>(e =>
        {
            e.HasKey(nf => nf.Id);
            e.Property(nf => nf.NumeroSequencial).ValueGeneratedOnAdd();    //Gera o numero sequencial sempre que uma nota nova for criada
            e.Property(nf => nf.Status).IsRequired();
            
            e.HasMany(nf => nf.Itens)
                .WithOne()
                .HasForeignKey(nf => nf.NotaFiscalId)
                .OnDelete(DeleteBehavior.Cascade);      //Caso a nota seja excluida deleta os itens
            
            e.Metadata.FindNavigation(nameof(NotaFiscal.Itens)) //Como a coleção de itens é privada
                ?.SetPropertyAccessMode(PropertyAccessMode.Field); //precisamos configurar o acesso via campo para o EF Core
                                                                    //conseguir mapear corretamente
        });
        
        modelBuilder.Entity<NotaFiscalItem>(e =>
        {
            e.HasKey(nfi => nfi.Id);
            e.Property(nfi => nfi.ProdutoCodigo).IsRequired().HasMaxLength(50);
            e.Property(nfi => nfi.Quantidade).IsRequired();
        });
        
        base.OnModelCreating(modelBuilder);
    }
}