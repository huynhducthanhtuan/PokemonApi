using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface ICountryRepository
    {
        bool CheckExistCountry(int countryId);
        ICollection<Country> GetCountries();
        Country GetCountry(int countryId);
        Country GetCountryByOwner(int ownerId);
        ICollection<Owner> GetOwnersFromACountry(int countryId);
        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country Country);
        //bool DeleteCountries(List<Country> countries);
        bool Save();
    }
}
