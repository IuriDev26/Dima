using System.Net.Http.Json;
using Dima.Core.Handlers;
using Dima.Core.Requests.Stripe;
using Dima.Core.Responses;
using Dima.Core.Responses.Stripe;

namespace Dima.App.Handlers;

public class StripeHandler(IHttpClientFactory httpClientFactory) : IStripeHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
    
    public async Task<Response<string?>> CreateSessionAsync(CreateSessionRequest request)
    {
        var response = await _client.PostAsJsonAsync("v1/payments/stripe/create-session", request);
        return await response.Content.ReadFromJsonAsync<Response<string?>>()
            ?? new Response<string?>(null, 500, "Erro ao criar sessão no stripe");
    }

    public async Task<Response<List<StripeTransactionResponse>>> GetTransactionsByOrderNumberAsync(GetTransactionsByOrderNumberRequest request)
        => await _client.GetFromJsonAsync<Response<List<StripeTransactionResponse>>>
               ($"v1/payments/stripe/get-transactions?orderNumber={request.Number}")
            ?? new Response<List<StripeTransactionResponse>>
                (null, 500, " Erro ao recuperar transações do stripe") ;
}