using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.DTOs;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PokemonRepository(
            DataContext context, 
            IMapper mapper, 
            IOwnerRepository ownerRepository,
            ICategoryRepository categoryRepository
        )
        {
            _context = context;
            _mapper = mapper;
            _ownerRepository = ownerRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> CheckExistPokemon(int pokemonId)
        {
            return await _context.Pokemons.AnyAsync(p => p.Id == pokemonId);
        }

        public async Task<bool> CheckExistPokemon(string pokemonName)
        {
            return await _context.Pokemons.AnyAsync(p => p.Name == pokemonName);
        }

        public async Task<IEnumerable<Pokemon>> GetPokemons()
        {
            return await _context.Pokemons.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<IEnumerable<PokemonDTO>> GetPokemonDTOs()
        {
            IEnumerable<Pokemon> pokemons = await GetPokemons();
            IEnumerable<PokemonDTO> pokemonDTOs = _mapper.Map<IEnumerable<PokemonDTO>>(pokemons);
            return pokemonDTOs;
        }

        public async Task<IEnumerable<Pokemon>> GetPokemonsByIds(int[] pokemonIds)
        {
            return await _context.Pokemons
                .Where(o => pokemonIds.Contains(o.Id))
                .ToListAsync();
        }
        public async Task<IEnumerable<PokemonDTO>> GetPokemonDTOsByIds(int[] pokemonIds)
        {
            IEnumerable<Pokemon> pokemons = await GetPokemonsByIds(pokemonIds);
            IEnumerable<PokemonDTO> pokemonDTOs = _mapper.Map<IEnumerable<PokemonDTO>>(pokemons);
            return pokemonDTOs;
        }

        public async Task<Pokemon> GetPokemon(int pokemonId)
        {
            return await _context.Pokemons.FirstOrDefaultAsync(p => p.Id == pokemonId);
        }

        public async Task<PokemonDTO> GetPokemonDTO(int pokemonId)
        {
            Pokemon pokemon = await GetPokemon(pokemonId);
            PokemonDTO pokemonDTO = _mapper.Map<PokemonDTO>(pokemon);
            return pokemonDTO;
        }

        public async Task<Pokemon> GetPokemon(string pokemonName)
        {
            return await _context.Pokemons.FirstOrDefaultAsync(p => p.Name == pokemonName);
        }

        public async Task<PokemonDTO> GetPokemonDTO(string pokemonName)
        {
            Pokemon pokemon = await GetPokemon(pokemonName);
            PokemonDTO pokemonDTO = _mapper.Map<PokemonDTO>(pokemon);
            return pokemonDTO;
        }

        public async Task<double> GetPokemonRatingPoint(int pokemonId)
        {
            IEnumerable<Review> reviewsOfPokemon = await _context.Reviews
                .Where(r => r.Pokemon.Id == pokemonId)
                .ToListAsync();

            int sumOfRating = reviewsOfPokemon.Sum(p => p.Rating);
            int countOfRating = reviewsOfPokemon.Count();

            if (sumOfRating <= 0)
                return 0;

            double ratingPoint = sumOfRating / countOfRating;
            return ratingPoint;
        }

        public async Task<bool> CreatePokemon(
            int ownerId, 
            int categoryId, 
            PokemonDTO pokemonCreate
        )
        {
            Owner owner = await _ownerRepository.GetOwner(ownerId);
            Category category = await _categoryRepository.GetCategory(categoryId);
            Pokemon pokemon = _mapper.Map<Pokemon>(pokemonCreate);    

            PokemonOwner pokemonOwner = new PokemonOwner()
            {
                Owner = owner,
                Pokemon = pokemon,
            };
            await _context.AddAsync(pokemonOwner);

            PokemonCategory pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon,
            };
            await _context.AddAsync(pokemonCategory);

            await _context.AddAsync(pokemonCreate);
            return Save();
        }

        public bool UpdatePokemon(PokemonDTO pokemon)
        {
            Pokemon pokemonToUpdate = _mapper.Map<Pokemon>(pokemon);
            _context.Update(pokemonToUpdate);
            return Save();
        }

        public async Task<bool> DeletePokemon(int pokemonId)
        {
            Pokemon pokemonToDelete = await GetPokemon(pokemonId);
            _context.Remove(pokemonToDelete);
            return Save();
        }

        public async Task<bool> DeletePokemons(int[] pokemonIds)
        {
            IEnumerable<Pokemon> pokemonsToDelete = await GetPokemonsByIds(pokemonIds);
            foreach (Pokemon pokemon in pokemonsToDelete)
            {
                _context.Remove(pokemon);
            }
            return Save();
        }

        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
