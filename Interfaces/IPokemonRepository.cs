using PokemonApi.DTOs;
using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface IPokemonRepository
    {
        bool CheckExistPokemon(int pokemonId);
        bool CheckExistPokemon(string pokemonName);
        IEnumerable<Pokemon> GetPokemons();
        IEnumerable<Pokemon> GetPokemonsByIds(int[] pokemonIds);
        Pokemon GetPokemon(int pokemonId);
        Pokemon GetPokemon(string pokemonName);
        double GetPokemonRating(int pokemonId);
        Pokemon GetPokemonTrimToUpper(PokemonDTO pokemonCreate);
        bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        bool DeletePokemon(Pokemon pokemon);
        bool DeletePokemons(IEnumerable<Pokemon> pokemons);
        bool Save();
    }
}
