using System.Net.Http.Json;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using MudBlazor;

namespace Dima.App.Handlers;

public class OrderHandler(IHttpClientFactory httpClientFactory) : IOrderHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);

    public async Task<Response<Order?>> CancelAsync(CancelOrderRequest request)
    {
        var response = await _client.PostAsJsonAsync("v1/orders/cancel", request);  
        return await response.Content.ReadFromJsonAsync<Response<Order?>>()
            ?? new Response<Order?>(null, 400, "Erro ao cancelar pedido");
    }
    
    public async Task<Response<Order?>> CreateAsync(CreateOrderRequest request)
    {
        var response = await _client.PostAsJsonAsync<CreateOrderRequest>("v1/orders/create", request);  
        return await response.Content.ReadFromJsonAsync<Response<Order?>>()
               ?? new Response<Order?>(null, 400, "Erro ao criar pedido");
    }

    public async Task<PagedResponse<List<Order>?>> GetAllAsync(GetAllOrdersRequest request)
        => await _client.GetFromJsonAsync<PagedResponse<List<Order>?>>("v1/orders")
           ?? new PagedResponse<List<Order>?>(null, 400, "Erro ao buscar pedidos");

    public async Task<Response<Order?>> GetByNumberAsync(GetOrderByNumberRequest request)
        => await _client.GetFromJsonAsync<Response<Order?>>($"v1/orders/{request.Number}")
           ?? new Response<Order?>(null, 400, "Erro ao buscar pedidos");

    public async Task<Response<Order?>> PayAsync(PayOrderRequest request)
    {
        var response = await _client.PostAsJsonAsync("v1/orders/pay", request);  
        return await response.Content.ReadFromJsonAsync<Response<Order?>>()
               ?? new Response<Order?>(null, 400, "Erro ao pagar pedido");
    }

    public async Task<Response<Order?>> RefundAsync(RefundOrderRequest request)
    {
        var response = await _client.PostAsJsonAsync("v1/orders/refund", request);  
        return await response.Content.ReadFromJsonAsync<Response<Order?>>()
               ?? new Response<Order?>(null, 400, "Erro ao estornar pedido");
    }
}