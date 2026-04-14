namespace Faturamento.Application.DTOs;

public class NotaFiscalForRegistrationDTO
{
    public List<NotaFiscalItemDTO> Itens { get; set; } = new();
}