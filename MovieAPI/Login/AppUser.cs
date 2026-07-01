using Microsoft.AspNetCore.Identity;
namespace MovieAPI.Login
{

    public class AppUser : IdentityUser
    {
        public string? DisplayName { get; set; }
    }
}
