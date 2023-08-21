using PokemonApi.DTOs;

namespace PokemonApi.Interfaces
{
    public interface ICountryRepository
    {
        Task<bool> CheckExistCountry(int countryId);
        Task<IEnumerable<CountryDTO>> GetCountries();
        Task<CountryDTO> GetCountry(int countryId);
        Task<CountryDTO> GetCountry(string countryName);
        Task<CountryDTO> GetCountryByOwner(int ownerId);
        Task<IEnumerable<OwnerDTO>> GetOwnersFromCountry(int countryId);
        bool CreateCountry(CountryDTO country);
        bool UpdateCountry(CountryDTO country);
        Task<bool> DeleteCountry(int countryId);
        bool Save();
    }
}
