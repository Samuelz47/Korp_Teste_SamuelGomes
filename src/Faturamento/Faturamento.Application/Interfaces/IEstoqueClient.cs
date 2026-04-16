using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Faturamento.Application.DTOs;

namespace Faturamento.Application.Interfaces;

public interface IEstoqueClient
{
    Task<bool> AbaterSaldoAsync(Guid notaFiscalId, List<NotaFiscalItemDTO> itens);
}