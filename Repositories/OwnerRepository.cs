using Microsoft.EntityFrameworkCore;
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
        
        public bool CheckExistOwner(int ownerId)
        {
            return _context.Owners.Any(o => o.Id == ownerId);
        }

        public IEnumerable<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public IEnumerable<Owner> GetOwnersByIds(int[] ownerIds)
        {
            return _context.Owners
                .Where(o => ownerIds.Contains(o.Id))
                .ToList();
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners
                .Where(o => o.Id == ownerId)
                .FirstOrDefault();
        }

        public IEnumerable<Owner> GetOwnerOfPokemon(int pokemonId)
        {
            return (ICollection<Owner>)_context.PokemonOwners
                .Where(p => p.Pokemon.Id == pokemonId)
                .Include(o => o.Owner)
                .ToList();
        }

        public IEnumerable<Pokemon> GetPokemonsByOwner(int ownerId)
        {
            return _context.PokemonOwners
                .Where(p => p.Owner.Id == ownerId)
                .Select(p => p.Pokemon)
                .ToList();
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return Save();
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _context.Remove(owner);
            return Save();
        }

        public bool DeleteOwners(IEnumerable<Owner> owners)
        {
            foreach (Owner owner in owners)
            {
                _context.Remove(owner);
            }
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
