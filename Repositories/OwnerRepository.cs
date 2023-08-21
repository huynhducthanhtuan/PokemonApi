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

        public async Task<IEnumerable<Owner>> GetOwners()
        {
            return await _context.Owners.ToListAsync();
        }

        public async Task<IEnumerable<OwnerDTO>> GetOwnerDTOs()
        {
            IEnumerable<Owner> owners = await GetOwners();
            IEnumerable<OwnerDTO> ownerDTOs = _mapper.Map<IEnumerable<OwnerDTO>>(owners);
            return ownerDTOs;
        }

        public async Task<IEnumerable<Owner>> GetOwnersByIds(int[] ownerIds)
        {
            return await _context.Owners
                .Where(o => ownerIds.Contains(o.Id))
                .ToListAsync();
        }

        public async Task<IEnumerable<OwnerDTO>> GetOwnerDTOsByIds(int[] ownerIds)
        {
            IEnumerable<Owner> owners = await GetOwnersByIds(ownerIds);
            IEnumerable<OwnerDTO> ownerDTOs = _mapper.Map<IEnumerable<OwnerDTO>>(owners);
            return ownerDTOs;
        }

        public async Task<Owner> GetOwner(int ownerId)
        {
            return await _context.Owners
                .Where(o => o.Id == ownerId)
                .FirstOrDefaultAsync();
        }

        public async Task<OwnerDTO> GetOwnerDTO(int ownerId)
        {
            Owner owner = await GetOwner(ownerId);
            OwnerDTO ownerDTO = _mapper.Map<OwnerDTO>(owner);
            return ownerDTO;
        }

        public async Task<Owner> GetOwner(string ownerFirstName, string ownerLastName)
        {
            return await _context.Owners
                .Where(o => o.FirstName == ownerFirstName && o.LastName == ownerLastName)
                .FirstOrDefaultAsync();
        }

        public async Task<OwnerDTO> GetOwnerDTO(string ownerFirstName, string ownerLastName)
        {
            Owner owner = await GetOwner(ownerFirstName, ownerLastName);
            OwnerDTO ownerDTO = _mapper.Map<OwnerDTO>(owner);
            return ownerDTO;
        }

        public async Task<Owner> GetOwnerOfPokemon(int pokemonId)
        {
            return await _context.PokemonOwners
                .Where(o => o.Pokemon.Id == pokemonId)
                .Select(o => o.Owner)
                .FirstOrDefaultAsync();
        }

        public async Task<OwnerDTO> GetOwnerDTOOfPokemon(int pokemonId)
        {
            Owner owner = await GetOwnerOfPokemon(pokemonId);
            OwnerDTO ownerDTO = _mapper.Map<OwnerDTO>(owner);
            return ownerDTO;
        }

        public async Task<IEnumerable<Pokemon>> GetPokemonsByOwner(int ownerId)
        {
            return await _context.PokemonOwners
                .Where(p => p.Owner.Id == ownerId)
                .Select(p => p.Pokemon)
                .ToListAsync();
        }

        public async Task<IEnumerable<PokemonDTO>> GetPokemonDTOsByOwner(int ownerId)
        {
            IEnumerable<Pokemon> pokemons = await GetPokemonsByOwner(ownerId);
            IEnumerable<PokemonDTO> pokemonDTOs = _mapper.Map<IEnumerable<PokemonDTO>>(pokemons);
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
            Owner ownerToDelete = await GetOwner(ownerId);
            _context.Remove(ownerToDelete);
            return Save();
        }

        public async Task<bool> DeleteOwners(int[] ownerIds)
        {
            IEnumerable<Owner> ownersToDelete = await GetOwnersByIds(ownerIds);
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