using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
        Pokemon GetPokemon(int pokemonId);
        Pokemon GetPokemon(string pokemonName);
        double GetPokemonRating(int pokemonId);
        bool PokemonExists(int pokemonId);
        bool PokemonExists(string pokemonName);
    }
}
