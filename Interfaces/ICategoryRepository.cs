using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface ICategoryRepository
    {
        bool CategoryExists(int categoryId);
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);
        ICollection<Pokemon> GetPokemonByCategory(int categoryId);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        // bool DeleteCategories(List<Category> categories);
        bool Save();
    }
}
