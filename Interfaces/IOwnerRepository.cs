using PokemonApi.DTOs;
using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface IOwnerRepository
    {
        Task<bool> CheckExistOwner(int ownerId);
        Task<IEnumerable<Owner>> GetOwners();
        Task<IEnumerable<OwnerDTO>> GetOwnerDTOs();
        Task<IEnumerable<Owner>> GetOwnersByIds(int[] ownerIds);
        Task<IEnumerable<OwnerDTO>> GetOwnerDTOsByIds(int[] ownerIds);
        Task<Owner> GetOwner(int ownerId);
        Task<OwnerDTO> GetOwnerDTO(int ownerId);
        Task<Owner> GetOwner(string ownerFirstName, string ownerLastName);
        Task<OwnerDTO> GetOwnerDTO(string ownerFirstName, string ownerLastName);
        Task<Owner> GetOwnerOfPokemon(int pokemonId);
        Task<OwnerDTO> GetOwnerDTOOfPokemon(int pokemonId);
        Task<IEnumerable<Pokemon>> GetPokemonsByOwner(int ownerId);
        Task<IEnumerable<PokemonDTO>> GetPokemonDTOsByOwner(int ownerId);
        Task<bool> CreateOwner(OwnerDTO owner, CountryDTO country);
        bool UpdateOwner(OwnerDTO owner);
        Task<bool> DeleteOwner(int ownerId);
        Task<bool> DeleteOwners(int[] ownerIds);
        bool Save();
    }
}
