using Faturamento.Application.DTOs;
using Faturamento.Domain.Entities;

namespace Faturamento.Application.Interfaces;

public interface INotaFiscalService
{
    Task<NotaFiscal> GerarNotaFiscalAsync(NotaFiscalForRegistrationDTO notaFiscalDto);
}