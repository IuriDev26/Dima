using System.Net.Http.Json;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;

namespace Dima.App.Handlers;

public class ProductHandler(IHttpClientFactory httpClientFactory) : IProductHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);

    public async Task<PagedResponse<List<Product>?>> GetAllAsync(GetAllProductsRequest request)
        => await _client.GetFromJsonAsync<PagedResponse<List<Product>?>>("v1/products")
           ?? new PagedResponse<List<Product>?>(null, 400, "Erro ao buscar produtos");

    public async Task<Response<Product?>> GetByIdAsync(GetProductByIdRequest request)
        => await _client.GetFromJsonAsync<Response<Product?>>($"v1/products/{request.Id}")
           ?? new Response<Product?>(null, 400, "Erro ao buscar produto");
}