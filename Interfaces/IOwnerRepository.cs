using PokemonApi.DTOs;

namespace PokemonApi.Interfaces
{
    public interface IOwnerRepository
    {
        Task<bool> CheckExistOwner(int ownerId);
        Task<IEnumerable<OwnerDTO>> GetOwners();
        Task<IEnumerable<OwnerDTO>> GetOwnersByIds(int[] ownerIds);
        Task<OwnerDTO> GetOwner(int ownerId);
        Task<OwnerDTO> GetOwner(string ownerFirstName, string ownerLastName);
        Task<OwnerDTO> GetOwnerOfPokemon(int pokemonId);
        Task<IEnumerable<PokemonDTO>> GetPokemonsByOwner(int ownerId);
        Task<bool> CreateOwner(OwnerDTO owner, CountryDTO country);
        bool UpdateOwner(OwnerDTO owner);
        Task<bool> DeleteOwner(int ownerId);
        Task<bool> DeleteOwners(int[] ownerIds);
        bool Save();
    }
}
