using PokemonApi.Data;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners
                .Where(o => o.Id == ownerId)
                .FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int pokemonId)
        {
            return _context.PokemonOwners
                .Where(p => p.Pokemon.Id == pokemonId)
                .Select(o => o.Owner)
                .ToList();
        }


        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _context.PokemonOwners
                .Where(p => p.Owner.Id == ownerId)
                .Select(p => p.Pokemon)
                .ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return _context.Owners.Any(o => o.Id == ownerId);
        }
    }
}
