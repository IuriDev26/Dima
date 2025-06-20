using Microsoft.AspNetCore.Identity;

namespace Dima.Api.Models.Identity;

public class User : IdentityUser<long>
{
    public List<IdentityRole<long>> Roles { get; set; } = [];
}