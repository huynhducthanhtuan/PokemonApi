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

        public async Task<IEnumerable<CategoryDTO>> GetCategories()
        {
            IEnumerable<Category> categories = await _context.Categories.ToListAsync();
            IEnumerable<CategoryDTO> categoryDTOs = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
            return categoryDTOs;
        }

        public async Task<CategoryDTO> GetCategory(int categoryId)
        {
            Category category = await _context.Categories
                .Where(c => c.Id == categoryId)
                .FirstOrDefaultAsync();
            CategoryDTO categoryDTO = 
                _mapper.Map<CategoryDTO>(category);
            return categoryDTO;
        }

        public async Task<CategoryDTO> GetCategory(string categoryName)
        {
            Category category = await _context.Categories
                .Where(c => c.Name == categoryName)
                .FirstOrDefaultAsync();
            CategoryDTO categoryDTO = 
                _mapper.Map<CategoryDTO>(category);
            return categoryDTO;
        }

        public async Task<IEnumerable<PokemonDTO>> GetPokemonsByCategory(int categoryId)
        {
            IEnumerable<Pokemon> pokemons = await _context.PokemonCategories
                .Where(pc => pc.CategoryId == categoryId)
                .Select(pc => pc.Pokemon)
                .ToListAsync();
            IEnumerable<PokemonDTO> pokemonDTOs = 
                _mapper.Map<IEnumerable<PokemonDTO>>(pokemons);
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
            CategoryDTO category = await GetCategory(categoryId);
            Category categoryToDelete = _mapper.Map<Category>(category);
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
