using PokemonApi.DTOs;
using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface ICategoryRepository
    {
        Task<bool> CheckExistCategory(int categoryId);
        Task<IEnumerable<Category>> GetCategories();
        Task<IEnumerable<CategoryDTO>> GetCategoryDTOs();
        Task<Category> GetCategory(int categoryId);
        Task<CategoryDTO> GetCategoryDTO(int categoryId);
        Task<Category> GetCategory(string categoryName);
        Task<CategoryDTO> GetCategoryDTO(string categoryName);
        Task<IEnumerable<Pokemon>> GetPokemonsByCategory(int categoryId);
        Task<IEnumerable<PokemonDTO>> GetPokemonDTOsByCategory(int categoryId);
        bool CreateCategory(CategoryDTO category);
        bool UpdateCategory(CategoryDTO category);
        Task<bool> DeleteCategory(int categoryId);
        bool Save();
    }
}
