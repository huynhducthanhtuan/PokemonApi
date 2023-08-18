using PokemonApi.DTOs;
using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface IPokemonRepository
    {
        bool PokemonExists(int pokemonId);
        bool PokemonExists(string pokemonName);
        ICollection<Pokemon> GetPokemons();
        Pokemon GetPokemon(int pokemonId);
        Pokemon GetPokemon(string pokemonName);
        double GetPokemonRating(int pokemonId);
        Pokemon GetPokemonTrimToUpper(PokemonDTO pokemonCreate);
        bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        bool DeletePokemon(Pokemon pokemon);
        // bool DeletePokemons(List<Pokemon> pokemons);
        bool Save();
    }
}
