using System.Net.Http.Json;
using Dima.Core.Handlers;
using Dima.Core.Requests.Identity;
using Dima.Core.Responses;

namespace Dima.App.Handlers;

public class IdentityHandler(IHttpClientFactory httpClientFactory) : IIdentityHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
    
    public async Task<Response<string>> LoginAsync(LoginRequest request)
    {
        var response = await _client.PostAsJsonAsync("v1/identity/login?useCookies=true", request);
        return response.IsSuccessStatusCode
            ? new Response<string>(null, (int)response.StatusCode, "Login realizado com sucesso!")
            : new Response<string>(null, (int)response.StatusCode, "Não foi possível realizar o login");
    }

    public async Task<Response<string>> RegisterAsync(RegisterRequest request)
    {
        var response = await _client.PostAsJsonAsync("v1/identity/register", request);

        if (!response.IsSuccessStatusCode)
            return new Response<string>(null, (int)response.StatusCode, "Erro ao criar conta");
        
        var loginRequest = new LoginRequest()
        {
            Email = request.Email,
            Password = request.Password
        };

        await LoginAsync(loginRequest);
        return new Response<string>(null, (int)response.StatusCode, "Conta criada com sucesso");

    }

    public async Task<Response<string>> LogoutAsync()
    {
        var response = await _client.GetAsync("v1/identity/logout");
        return response.IsSuccessStatusCode
            ? new Response<string>(null, (int)response.StatusCode, "Logout realizado com sucesso!")
            : new Response<string>(null, (int)response.StatusCode, "Erro ao fazer logout");
    }
}