using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.DTOs;
using PokemonApi.Interfaces;

namespace PokemonApi.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        ///<summary>Get List Of Categories</summary>
        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCategories()
        {
            IEnumerable<CategoryDTO> categories =
                await _categoryRepository.GetCategoryDTOs();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(categories);
        }

        ///<summary>Get Category By Id</summary>
        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(CategoryDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            if (categoryId == null)
                return BadRequest();

            if (!await _categoryRepository.CheckExistCategory(categoryId))
                return NotFound();

            CategoryDTO category =
                await _categoryRepository.GetCategoryDTO(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }

        ///<summary>Get Pokemons By Category Id</summary>
        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPokemonsByCategory(int categoryId)
        {
            if (categoryId == null)
                return BadRequest();

            IEnumerable<PokemonDTO> pokemons =
                await _categoryRepository.GetPokemonDTOsByCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(pokemons);
        }

        ///<summary>Create Category</summary>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateCategory(
            [FromBody] CategoryDTO categoryCreate
        )
        {
            if (categoryCreate == null)
                return BadRequest(ModelState);

            CategoryDTO category =
                await _categoryRepository.GetCategoryDTO(categoryCreate.Name);

            if (category != null)
            {
                ModelState.AddModelError("error", "Category already exists");
                return Conflict(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.CreateCategory(categoryCreate))
            {
                ModelState.AddModelError("error", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction("GetCategory", new { categoryId = categoryCreate.Id });
        }

        ///<summary>Update Category</summary>
        [HttpPut("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateCategory(
            int categoryId,
            [FromBody] CategoryDTO updateCategory
        )
        {
            if (categoryId == null || updateCategory == null)
                return BadRequest(ModelState);

            if (categoryId != updateCategory.Id)
                return BadRequest(ModelState);

            if (!await _categoryRepository.CheckExistCategory(categoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_categoryRepository.UpdateCategory(updateCategory))
            {
                ModelState.AddModelError("error", "Something went wrong updating category");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        ///<summary>Delete Category</summary>
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            if (categoryId == null)
                return BadRequest();

            if (!await _categoryRepository.CheckExistCategory(categoryId))
                return NotFound();

            if (!await _categoryRepository.DeleteCategory(categoryId))
            {
                ModelState.AddModelError("error", "Something went wrong when deleting category");
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
