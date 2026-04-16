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
    
    public async Task<bool> AbaterSaldoAsync(Guid notaFiscalId, List<NotaFiscalItemDTO> itens)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, "/api/Produtos")
        {
            Content = JsonContent.Create(itens)
        };
        request.Headers.Add("X-Idempotency-Key", notaFiscalId.ToString());

        var response = await _httpClient.SendAsync(request);
        
        return response.IsSuccessStatusCode;
    }
}