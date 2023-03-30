using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
        Pokemon GetPokemon(int id);
        Pokemon GetPokemon(string name);
        double GetPokemonRating(int pokemonId);
        bool PokemonExists(int pokemonId);
        bool PokemonExists(string pokemonName);
    }
}
