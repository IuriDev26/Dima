using System.Net.Http.Json;
using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.App.Handlers;

public class TransactionHandler(IHttpClientFactory httpClientFactory) : ITransactionHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
    
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        var response = await _client.PostAsJsonAsync("v1/transactions", request);
        return await response.Content.ReadFromJsonAsync<Response<Transaction?>>() 
                          ?? new Response<Transaction?>(null, 500, "Falha ao criar Transação");
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        var response = await _client.PutAsJsonAsync($"$v1/transactions/{request.Id}", request);
        return await response.Content.ReadFromJsonAsync<Response<Transaction?>>() 
               ?? new Response<Transaction?>(null, 500, "Falha ao atualizar Transação");
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        var response = await _client.DeleteAsync($"$v1/transactions/{request.Id}");
        return await response.Content.ReadFromJsonAsync<Response<Transaction?>>() 
               ?? new Response<Transaction?>(null, 500, "Falha ao deletar Transação");
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetByIdRequest request)
    {
        var response = await _client.GetAsync($"$v1/transactions/{request.Id}");
        return await response.Content.ReadFromJsonAsync<Response<Transaction?>>() 
               ?? new Response<Transaction?>(null, 500, "Falha ao buscar Transação");
    }

    public async Task<PagedResponse<List<Transaction?>>> GetByPeriodAsync(GetByPeriodRequest request)
    {
        var format = "yyyy-MM-dd";
        
        var pageNumber = request.PageNumber <= 0 
            ? Configuration.DefaultPageNumber
            : request.PageNumber;
        
        var pageSize = request.PageSize <= 0 
            ? Configuration.DefaultPageSize
            : request.PageSize;

        var startDate = (request.InitialInterval ?? DateTime.Now.FirstDay()).ToString(format);
        var finalDate = (request.FinalInterval ?? DateTime.Now.LastDay()).ToString(format);
        
        var url = $"v1/transactions?pageSize={pageSize}&pageNumber={pageNumber}&startDate={startDate}&finalDate={finalDate}";
        
        var response = await _client.GetAsync(url);
        return await response.Content.ReadFromJsonAsync<PagedResponse<List<Transaction?>>>()
               ?? new PagedResponse<List<Transaction?>>(null, 500, "Erro ao buscar transações no período especificado");

    }
}