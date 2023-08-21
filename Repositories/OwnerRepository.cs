using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.DTOs;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public OwnerRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckExistOwner(int ownerId)
        {
            return await _context.Owners.AnyAsync(o => o.Id == ownerId);
        }

        public async Task<IEnumerable<OwnerDTO>> GetOwners()
        {
            IEnumerable<Owner> owners = await _context.Owners.ToListAsync();
            IEnumerable<OwnerDTO> ownerDTOs = _mapper.Map<IEnumerable<OwnerDTO>>(owners);
            return ownerDTOs;
        }

        public async Task<IEnumerable<OwnerDTO>> GetOwnersByIds(int[] ownerIds)
        {
            IEnumerable<Owner> owners = await _context.Owners
                .Where(o => ownerIds.Contains(o.Id))
                .ToListAsync();
            IEnumerable<OwnerDTO> ownerDTOs =
                _mapper.Map<IEnumerable<OwnerDTO>>(owners);
            return ownerDTOs;
        }

        public async Task<OwnerDTO> GetOwner(int ownerId)
        {
            Owner owner = await _context.Owners
                .Where(o => o.Id == ownerId)
                .FirstOrDefaultAsync();
            OwnerDTO ownerDTO = 
                _mapper.Map<OwnerDTO>(owner);
            return ownerDTO;
        }

        public async Task<OwnerDTO> GetOwner(string ownerFirstName, string ownerLastName)
        {
            Owner owner = await _context.Owners
                .Where(o => o.FirstName == ownerFirstName && o.LastName == ownerLastName)
                .FirstOrDefaultAsync();
            OwnerDTO ownerDTO = 
                _mapper.Map<OwnerDTO>(owner);
            return ownerDTO;
        }

        public async Task<OwnerDTO> GetOwnerOfPokemon(int pokemonId)
        {
            Owner owner = await _context.PokemonOwners
                .Where(p => p.Pokemon.Id == pokemonId)
                .Select(o => o.Owner)
                .FirstOrDefaultAsync();
            OwnerDTO ownerDTO = 
                _mapper.Map<OwnerDTO>(owner);
            return ownerDTO;
        }

        public async Task<IEnumerable<PokemonDTO>> GetPokemonsByOwner(int ownerId)
        {
            IEnumerable<Pokemon> pokemons = await _context.PokemonOwners
                .Where(p => p.Owner.Id == ownerId)
                .Select(p => p.Pokemon)
                .ToListAsync();
            IEnumerable<PokemonDTO> pokemonDTOs =
                _mapper.Map<IEnumerable<PokemonDTO>>(pokemons);
            return pokemonDTOs;
        }

        public async Task<bool> CreateOwner(OwnerDTO owner, CountryDTO country)
        {
            Owner ownerToCreate = _mapper.Map<Owner>(owner);
            Country countryToCreate = _mapper.Map<Country>(country);

            ownerToCreate.Country = countryToCreate;
            await _context.AddAsync(owner);

            return Save();
        }

        public bool UpdateOwner(OwnerDTO owner)
        {
            Owner ownerToUpdate = _mapper.Map<Owner>(owner);
            _context.Update(ownerToUpdate);
            return Save();
        }

        public async Task<bool> DeleteOwner(int ownerId)
        {
            OwnerDTO owner = await GetOwner(ownerId);
            Owner ownerToDelete = _mapper.Map<Owner>(owner);
            _context.Remove(ownerToDelete);
            return Save();
        }

        public async Task<bool> DeleteOwners(int[] ownerIds)
        {
            IEnumerable<OwnerDTO> owners = await GetOwnersByIds(ownerIds);
            IEnumerable<Owner> ownersToDelete = _mapper.Map<IEnumerable<Owner>>(owners);

            foreach (Owner owner in ownersToDelete)
            {
                _context.Remove(owner);
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