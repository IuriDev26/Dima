using System.Net.Http.Json;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.App.Handlers;

public class CategoryHandler(IHttpClientFactory httpClientFactory) : ICategoryHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
    
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        var response = await _client.PostAsJsonAsync("v1/categories", request);
        return await response.Content.ReadFromJsonAsync<Response<Category?>>()
            ?? new Response<Category?>(null, 500, "Erro ao criar categoria");
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        var response = await _client.PutAsJsonAsync($"v1/categories/{request.Id}", request);
        return await response.Content.ReadFromJsonAsync<Response<Category?>>()
               ?? new Response<Category?>(null, 500, "Erro ao atualizar categoria");
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        var response = await _client.DeleteAsync($"v1/categories/{request.Id}");
        return await response.Content.ReadFromJsonAsync<Response<Category?>>()
               ?? new Response<Category?>(null, 500, "Erro ao deletar categoria");
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    => await _client.GetFromJsonAsync<Response<Category?>>($"v1/categories/{request.Id}")
        ?? new Response<Category?>(null, 500, "Erro ao buscar categoria");

    public async Task<PagedResponse<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
    {
        var pageNumber = request.PageNumber == 0 
            ? Configuration.DefaultPageNumber 
            : request.PageNumber;
        var pageSize = request.PageSize == 0 
            ? Configuration.DefaultPageSize 
            : request.PageSize;
        
        var url = $"v1/categories?pageSize={pageSize}&pageNumber={pageNumber}";
        
        return await _client.GetFromJsonAsync<PagedResponse<List<Category>>>(url)
            ?? new PagedResponse<List<Category>>(null, 500, "Erro ao buscar categorias");
    }
}