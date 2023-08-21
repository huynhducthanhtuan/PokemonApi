using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.DTOs;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckExistCategory(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId);
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoryDTOs()
        {
            IEnumerable<Category> categories = await GetCategories();
            IEnumerable<CategoryDTO> categoryDTOs = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
            return categoryDTOs;
        }

        public async Task<Category> GetCategory(int categoryId)
        {
            return await _context.Categories
                .Where(c => c.Id == categoryId)
                .FirstOrDefaultAsync();
        }

        public async Task<CategoryDTO> GetCategoryDTO(int categoryId)
        {
            Category category = await GetCategory(categoryId);
            CategoryDTO categoryDTO = _mapper.Map<CategoryDTO>(category);
            return categoryDTO;
        }

        public async Task<Category> GetCategory(string categoryName)
        {
            return await _context.Categories
                .Where(c => c.Name == categoryName)
                .FirstOrDefaultAsync();
        }

        public async Task<CategoryDTO> GetCategoryDTO(string categoryName)
        {
            Category category = await GetCategory(categoryName);
            CategoryDTO categoryDTO = _mapper.Map<CategoryDTO>(category);
            return categoryDTO;
        }

        public async Task<IEnumerable<Pokemon>> GetPokemonsByCategory(int categoryId)
        {
            return await _context.PokemonCategories
                .Where(pc => pc.CategoryId == categoryId)
                .Select(pc => pc.Pokemon)
                .ToListAsync();
        }

        public async Task<IEnumerable<PokemonDTO>> GetPokemonDTOsByCategory(int categoryId)
        {
            IEnumerable<Pokemon> pokemons = await GetPokemonsByCategory(categoryId);
            IEnumerable<PokemonDTO> pokemonDTOs = _mapper.Map<IEnumerable<PokemonDTO>>(pokemons);
            return pokemonDTOs;
        }

        public bool CreateCategory(CategoryDTO category)
        {
            Category categoryToCreate = _mapper.Map<Category>(category);
            _context.Add(categoryToCreate);
            return Save();
        }

        public bool UpdateCategory(CategoryDTO category)
        {
            Category categoryToUpdate = _mapper.Map<Category>(category);
            _context.Update(categoryToUpdate);
            return Save();
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            Category categoryToDelete = await GetCategory(categoryId);
            _context.Remove(categoryToDelete);
            return Save();
        }

        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
