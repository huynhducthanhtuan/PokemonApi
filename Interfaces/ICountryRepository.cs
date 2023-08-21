using PokemonApi.DTOs;
using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface ICountryRepository
    {
        Task<bool> CheckExistCountry(int countryId);
        Task<IEnumerable<Country>> GetCountries();
        Task<IEnumerable<CountryDTO>> GetCountryDTOs();
        Task<Country> GetCountry(int countryId);
        Task<CountryDTO> GetCountryDTO(int countryId);
        Task<Country> GetCountry(string countryName);
        Task<CountryDTO> GetCountryDTO(string countryName);
        Task<Country> GetCountryByOwner(int ownerId);
        Task<CountryDTO> GetCountryDTOByOwner(int ownerId);
        Task<IEnumerable<Owner>> GetOwnersFromCountry(int countryId);
        Task<IEnumerable<OwnerDTO>> GetOwnerDTOsFromCountry(int countryId);
        bool CreateCountry(CountryDTO country);
        bool UpdateCountry(CountryDTO country);
        Task<bool> DeleteCountry(int countryId);
        bool Save();
    }
}
