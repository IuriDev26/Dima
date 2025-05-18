using System.Net.Http.Json;
using System.Security.Claims;
using Dima.Core.Models.Identity;
using Microsoft.AspNetCore.Components.Authorization;

namespace Dima.App.Security;

public class CookieAuthenticationStateProvider(IHttpClientFactory httpClientFactory) 
    : AuthenticationStateProvider, ICookieAuthenticationStateProvider
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
    private bool _isAuthenticated = false;
    
    public async Task<bool> CheckAuthenticatedAsync()
    {
        await GetAuthenticationStateAsync();
        return _isAuthenticated;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _isAuthenticated = false;
        
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        
        var userInfo = await GetUserAsync();
        if (userInfo is null)
            return new AuthenticationState(user);
        
        var claims = await GetClaimsAsync(userInfo);

        var identity = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
        user = new ClaimsPrincipal(identity);
        
        _isAuthenticated = true;
        return new AuthenticationState(user);
    }

    public void NotifyAuthenticationStateChanged()
        => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    private async Task<User?> GetUserAsync()
    {
        try
        {
            var user = await _client.GetFromJsonAsync<User?>("v1/identity/manage/info");
            return user;
        }
        catch (Exception ex)
        {
            return null;
        }
    }    

    private async Task<List<Claim>> GetClaimsAsync(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Email, user.Email),
        };
        
        claims.AddRange(
        user.Claims.Where(x => x.Key != ClaimTypes.Email && 
                               x.Key != ClaimTypes.Name)
            .Select(x => new Claim(x.Key, x.Value)));
        
        var roles = await _client.GetFromJsonAsync<List<RoleClaim>>("v1/identity/roles");

        foreach (var role in roles ?? [])
        {
            if ( !string.IsNullOrEmpty(role.Type) && !string.IsNullOrEmpty(role.Value) )
                claims.Add(new Claim(role.Type, role.Value, role.ValueType, role.Issuer, role.OriginalIssuer));
        }
        
        return claims;
    }
    
}