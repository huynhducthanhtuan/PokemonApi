using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);
        ICollection<Pokemon> GetPokemonByCategory(int categoryId);
        bool CategoryExists(int categoryId);
    }
}
