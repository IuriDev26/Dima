namespace Dima.Core.Models.Identity;

public class User
{
    public string Email { get; set; } = string.Empty;
    public Dictionary<string, string> Claims { get; set; } = [];
}