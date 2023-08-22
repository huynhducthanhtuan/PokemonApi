using PokemonApi.Utils;

namespace PokemonApi.DTOs
{
    public class SignupDTO
    {
        public string UserRole { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
