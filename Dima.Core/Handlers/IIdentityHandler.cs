using Dima.Core.Requests.Identity;
using Dima.Core.Responses;

namespace Dima.Core.Handlers;

public interface IIdentityHandler
{
    Task<Response<string>> LoginAsync(LoginRequest request);
    Task<Response<string>> RegisterAsync(RegisterRequest request);
    Task<Response<string>> LogoutAsync();
}