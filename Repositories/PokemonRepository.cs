using PokemonApi.Data;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;

        public PokemonRepository(DataContext context) 
        {
            _context = context;
        }

        public ICollection<Pokemon> GetPokemons() 
        {
            return _context.Pokemons.OrderBy(p => p.Id).ToList();
        }
    }
}
