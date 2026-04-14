using Faturamento.Application.DTOs;

namespace Faturamento.Application.Interfaces;

public interface IEstoqueClient
{
    Task<bool> AbaterSaldoAsync(List<NotaFiscalItemDTO> itens);
}