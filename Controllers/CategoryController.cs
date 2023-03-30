using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        ///<summary>Get Category List</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDto>>
                (_categoryRepository.GetCategories());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(categories);
        }

        ///<summary>Get Category By Id</summary>
        [HttpGet("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var category = _mapper.Map<CategoryDto>
                (_categoryRepository.GetCategory(categoryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }

        ///<summary>Get Pokemon By Category Id</summary>
        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetPokemonByCategoryId(int categoryId)
        {
            var pokemons = _mapper.Map<List<PokemonDto>>
                (_categoryRepository.GetPokemonByCategory(categoryId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(pokemons);
        }
    }
}
