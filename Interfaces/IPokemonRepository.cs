using PokemonApi.DTOs;

namespace PokemonApi.Interfaces
{
    public interface IPokemonRepository
    {
        Task<bool> CheckExistPokemon(int pokemonId);
        Task<bool> CheckExistPokemon(string pokemonName);
        Task<IEnumerable<PokemonDTO>> GetPokemons();
        Task<IEnumerable<PokemonDTO>> GetPokemonsByIds(int[] pokemonIds);
        Task<PokemonDTO> GetPokemon(int pokemonId);
        Task<PokemonDTO> GetPokemon(string pokemonName);
        Task<double> GetPokemonRatingPoint(int pokemonId);
        Task<bool> CreatePokemon(int ownerId, int categoryId, PokemonDTO pokemonCreate);
        bool UpdatePokemon(PokemonDTO pokemonUpdate);
        Task<bool> DeletePokemon(int pokemonId);
        Task<bool> DeletePokemons(int[] pokemonIds);
        bool Save();
    }
}
