using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface IOwnerRepository
    {
        bool OwnerExists(int ownerId);
        ICollection<Owner> GetOwners();
        Owner GetOwner(int ownerId);
        ICollection<Owner> GetOwnerOfAPokemon(int pokemonId);
        ICollection<Pokemon> GetPokemonByOwner(int ownerId);
        bool CreateOwner(Owner owner);
        bool UpdateOwner(Owner owner);
        bool DeleteOwner(Owner owner);
        // bool DeleteOwners(List<Owner> owners);
        bool Save();
    }
}
