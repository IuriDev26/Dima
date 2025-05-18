namespace Dima.Core.Requests.Identity;

public class LoginRequest : Request
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}