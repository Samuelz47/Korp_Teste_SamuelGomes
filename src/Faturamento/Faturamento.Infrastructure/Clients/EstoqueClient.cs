using System.Net.Http.Json;
using Faturamento.Application.DTOs;
using Faturamento.Application.Interfaces;

namespace Faturamento.Infrastructure.Clients;

public class EstoqueClient : IEstoqueClient
{
    private readonly HttpClient _httpClient;
    
    public EstoqueClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<bool> AbaterSaldoAsync(List<NotaFiscalItemDTO> itens)
    {
        var response = await _httpClient.PutAsJsonAsync("/api/Produtos", itens);
        
        return response.IsSuccessStatusCode;
    }
}