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

        public async Task<IEnumerable<CountryDTO>> GetCountries()
        {
            IEnumerable<Country> countries = await _context.Countries.ToListAsync();
            IEnumerable<CountryDTO> countryDTOs = _mapper.Map<IEnumerable<CountryDTO>>(countries);
            return countryDTOs;
        }

        public async Task<CountryDTO> GetCountry(int countryId)
        {
            Country country = await _context.Countries
                .Where(c => c.Id == countryId)
                .FirstOrDefaultAsync();
            CountryDTO countryDTO = _mapper.Map<CountryDTO>(country);
            return countryDTO;
        }

        public async Task<CountryDTO> GetCountry(string countryName)
        {
            Country country = await _context.Countries
                .Where(c => c.Name == countryName)
                .FirstOrDefaultAsync();
            CountryDTO countryDTO = _mapper.Map<CountryDTO>(country);
            return countryDTO;
        }

        public async Task<CountryDTO> GetCountryByOwner(int ownerId)
        {
            Country country = await _context.Owners
                .Where(o => o.Id == ownerId)
                .Select(o => o.Country)
                .FirstOrDefaultAsync();
            CountryDTO countryDTO = _mapper.Map<CountryDTO>(country);
            return countryDTO;
        }

        public async Task<IEnumerable<OwnerDTO>> GetOwnersFromCountry(int countryId)
        {
            IEnumerable<Owner> owners = await _context.Owners
                .Where(c => c.Country.Id == countryId)
                .ToListAsync();
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
            CountryDTO country = await GetCountry(countryId);
            Country countryToDelete = _mapper.Map<Country>(country);
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
