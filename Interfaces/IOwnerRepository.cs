using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface IOwnerRepository
    {
        bool CheckExistOwner(int ownerId);
        IEnumerable<Owner> GetOwners();
        IEnumerable<Owner> GetOwnersByIds(int[] ownerIds);
        Owner GetOwner(int ownerId);
        IEnumerable<Owner> GetOwnerOfPokemon(int pokemonId);
        IEnumerable<Pokemon> GetPokemonsByOwner(int ownerId);
        bool CreateOwner(Owner owner);
        bool UpdateOwner(Owner owner);
        bool DeleteOwner(Owner owner);
        bool DeleteOwners(IEnumerable<Owner> owners);
        bool Save();
    }
}
