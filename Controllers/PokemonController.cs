using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.DTOs;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Controllers
{
    [Route("api/pokemon")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public PokemonController(
            IPokemonRepository pokemonRepository, 
            IReviewRepository reviewRepository,
            IMapper mapper
        ) {
            _pokemonRepository = pokemonRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        ///<summary>Get Pokemon List</summary>
        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDTO>>
                (_pokemonRepository.GetPokemons());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(pokemons);
        }

        ///<summary>Get Pokemons By Pokemon Ids</summary>
        [HttpPost("list/ids")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult GetPokemonsByIds(int[] pokemonIds)
        {
            List<Pokemon> pokemons = _pokemonRepository.GetPokemonsByIds(pokemonIds).ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        ///<summary>Get Pokemon By Id</summary>
        [HttpGet("{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemon(int pokemonId)
        {
            if (!_pokemonRepository.PokemonExists(pokemonId))
                return NotFound();

            var pokemon = _mapper.Map<PokemonDTO>
                (_pokemonRepository.GetPokemon(pokemonId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(pokemon);
        }

        ///<summary>Get Pokemon By Name</summary>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemon(string pokemonName)
        {
            string _pokemonName = HttpContext.Request.Query["pokemonName"];

            if (!_pokemonRepository.PokemonExists(_pokemonName))
                return NotFound();

            var pokemon = _mapper.Map<PokemonDTO>
                (_pokemonRepository.GetPokemon(_pokemonName));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(pokemon);
        }

        ///<summary>Get Pokemon Rating</summary>
        [HttpGet("{pokemonId}/rating")]
        [ProducesResponseType(200, Type = typeof(double))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemonRating(int pokemonId)
        {
            if (!_pokemonRepository.PokemonExists(pokemonId))
                return NotFound();

            var rating = _pokemonRepository.GetPokemonRating(pokemonId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(rating);
        }

        ///<summary>Test Pass 2 Params In 1</summary>
        [HttpGet("{pokemonId}&{pokemonName}")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon([FromRoute] int pokemonId, [FromRoute] string pokemonName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(pokemonId + " " + pokemonName);
        }

        ///<summary>Create Pokemon</summary>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreatePokemon(
            [FromQuery] int ownerId, 
            [FromQuery] int categoryId, 
            [FromBody] PokemonDTO pokemonCreate
        ) {
            if (pokemonCreate == null)
                return BadRequest(ModelState);

            var pokemons = _pokemonRepository.GetPokemonTrimToUpper(pokemonCreate);

            if (pokemons != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

            if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        ///<summary>Update Pokemon</summary>
        [HttpPut("{pokemonId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdatePokemon(
            int pokemonId, 
            [FromQuery] int ownerId, 
            [FromQuery] int categoryId,
            [FromBody] PokemonDTO updatedPokemon
        ) {
            if (updatedPokemon == null)
                return BadRequest(ModelState);

            if (pokemonId != updatedPokemon.Id)
                return BadRequest(ModelState);

            if (!_pokemonRepository.PokemonExists(pokemonId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var pokemonMap = _mapper.Map<Pokemon>(updatedPokemon);

            if (!_pokemonRepository.UpdatePokemon(ownerId, categoryId,pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        ///<summary>Delete Pokemon</summary>
        [HttpDelete("{pokemonId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int pokemonId)
        {
            if (!_pokemonRepository.PokemonExists(pokemonId))
                return NotFound();

            var reviewsToDelete = _reviewRepository.GetReviewsOfAPokemon(pokemonId);
            var pokemonToDelete = _pokemonRepository.GetPokemon(pokemonId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
                ModelState.AddModelError("", "Something went wrong when deleting reviews");

            if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
                ModelState.AddModelError("", "Something went wrong deleting owner");

            return NoContent();
        }

        ///<summary>Delete Pokemons By Pokemon Ids</summary>
        [HttpDelete("ids")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeletePokemonsByIds(int[] pokemonIds)
        {
            List<Pokemon> pokemons = _pokemonRepository.GetPokemonsByIds(pokemonIds).ToList();

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_pokemonRepository.DeletePokemons(pokemons))
            {
                ModelState.AddModelError("", "error deleting pokemons");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
