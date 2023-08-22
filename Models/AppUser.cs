using Microsoft.AspNetCore.Identity;
using PokemonApi.Utils;

namespace PokemonApi.Models
{
    public class AppUser : IdentityUser
    {
        //public string Email { get; set; }
        //public string Password { get; set; }
        //public string UserName { get; set; }
        public string UserRole { get; set; }
    }
}
