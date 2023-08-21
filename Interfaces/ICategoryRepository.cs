using PokemonApi.DTOs;

namespace PokemonApi.Interfaces
{
    public interface ICategoryRepository
    {
        Task<bool> CheckExistCategory(int categoryId);
        Task<IEnumerable<CategoryDTO>> GetCategories();
        Task<CategoryDTO> GetCategory(int categoryId);
        Task<CategoryDTO> GetCategory(string categoryName);
        Task<IEnumerable<PokemonDTO>> GetPokemonsByCategory(int categoryId);
        bool CreateCategory(CategoryDTO category);
        bool UpdateCategory(CategoryDTO category);
        Task<bool> DeleteCategory(int categoryId);
        bool Save();
    }
}
