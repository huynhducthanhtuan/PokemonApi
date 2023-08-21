using PokemonApi.DTOs;
using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface IPokemonRepository
    {
        Task<bool> CheckExistPokemon(int pokemonId);
        Task<bool> CheckExistPokemon(string pokemonName);
        Task<IEnumerable<Pokemon>> GetPokemons();
        Task<IEnumerable<PokemonDTO>> GetPokemonDTOs();
        Task<IEnumerable<Pokemon>> GetPokemonsByIds(int[] pokemonIds);
        Task<IEnumerable<PokemonDTO>> GetPokemonDTOsByIds(int[] pokemonIds);
        Task<Pokemon> GetPokemon(int pokemonId);
        Task<PokemonDTO> GetPokemonDTO(int pokemonId);
        Task<Pokemon> GetPokemon(string pokemonName);
        Task<PokemonDTO> GetPokemonDTO(string pokemonName);
        Task<double> GetPokemonRatingPoint(int pokemonId);
        Task<bool> CreatePokemon(int ownerId, int categoryId, PokemonDTO pokemonCreate);
        bool UpdatePokemon(PokemonDTO pokemonUpdate);
        Task<bool> DeletePokemon(int pokemonId);
        Task<bool> DeletePokemons(int[] pokemonIds);
        bool Save();
    }
}
