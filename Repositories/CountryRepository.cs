using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.DTOs;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CountryRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckExistCountry(int countryId)
        {
            return await _context.Countries.AnyAsync(c => c.Id == countryId);
        }

        public async Task<IEnumerable<Country>> GetCountries()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<IEnumerable<CountryDTO>> GetCountryDTOs()
        {
            IEnumerable<Country> countries = await GetCountries();
            IEnumerable<CountryDTO> countryDTOs = _mapper.Map<IEnumerable<CountryDTO>>(countries);
            return countryDTOs;
        }

        public async Task<Country> GetCountry(int countryId)
        {
            return await _context.Countries
                .Where(c => c.Id == countryId)
                .FirstOrDefaultAsync();
        }

        public async Task<CountryDTO> GetCountryDTO(int countryId)
        {
            Country country = await GetCountry(countryId);
            CountryDTO countryDTO = _mapper.Map<CountryDTO>(country);
            return countryDTO;
        }

        public async Task<Country> GetCountry(string countryName)
        {
            return await _context.Countries
                .Where(c => c.Name == countryName)
                .FirstOrDefaultAsync();
        }

        public async Task<CountryDTO> GetCountryDTO(string countryName)
        {
            Country country = await GetCountry(countryName);
            CountryDTO countryDTO = _mapper.Map<CountryDTO>(country);
            return countryDTO;
        }

        public async Task<Country> GetCountryByOwner(int ownerId)
        {
            return await _context.Owners
                .Where(o => o.Id == ownerId)
                .Select(o => o.Country)
                .FirstOrDefaultAsync();
        }

        public async Task<CountryDTO> GetCountryDTOByOwner(int ownerId)
        {
            Country country = await GetCountryByOwner(ownerId);
            CountryDTO countryDTO = _mapper.Map<CountryDTO>(country);
            return countryDTO;
        }

        public async Task<IEnumerable<Owner>> GetOwnersFromCountry(int countryId)
        {
            return await _context.Owners
                .Where(c => c.Country.Id == countryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<OwnerDTO>> GetOwnerDTOsFromCountry(int countryId)
        {
            IEnumerable<Owner> owners = await GetOwnersFromCountry(countryId);
            IEnumerable<OwnerDTO> ownerDTOs = _mapper.Map<IEnumerable<OwnerDTO>>(owners);
            return ownerDTOs;
        }

        public bool CreateCountry(CountryDTO country)
        {
            Country countryToCreate = _mapper.Map<Country>(country);
            _context.Add(countryToCreate);
            return Save();
        }

        public bool UpdateCountry(CountryDTO country)
        {
            Country countryToUpdate = _mapper.Map<Country>(country);
            _context.Update(countryToUpdate);
            return Save();
        }

        public async Task<bool> DeleteCountry(int countryId)
        {
            Country countryToDelete = await GetCountry(countryId);
            _context.Remove(countryToDelete);
            return Save();
        }

        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
