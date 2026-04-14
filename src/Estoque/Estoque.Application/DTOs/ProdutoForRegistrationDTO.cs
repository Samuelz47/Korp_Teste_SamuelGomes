namespace Estoque.Application.DTOs;

public class ProdutoForRegistrationDTO
{
    public string Codigo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public int Saldo { get; set; }
}