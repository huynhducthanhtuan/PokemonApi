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

        public PokemonController(
            IPokemonRepository pokemonRepository,
            IReviewRepository reviewRepository
        )
        {
            _pokemonRepository = pokemonRepository;
            _reviewRepository = reviewRepository;
        }

        ///<summary>Get List Of Pokemons</summary>
        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDTO>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemons()
        {
            IEnumerable<PokemonDTO> pokemons = 
                await _pokemonRepository.GetPokemons();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        ///<summary>Get List Of Pokemons By Ids</summary>
        [HttpPost("list/ids")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPokemonsByIds(int[] pokemonIds)
        {
            if (pokemonIds == null || pokemonIds.Length == 0)
                return BadRequest();

            IEnumerable<PokemonDTO> pokemons =
                await _pokemonRepository.GetPokemonsByIds(pokemonIds);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        ///<summary>Get Pokemon By Id</summary>
        [HttpGet("{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(PokemonDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPokemon(int pokemonId)
        {
            if (pokemonId == null)
                return BadRequest();

            if (!await _pokemonRepository.CheckExistPokemon(pokemonId))
                return NotFound();

            PokemonDTO pokemon =
                await _pokemonRepository.GetPokemon(pokemonId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);
        }

        ///<summary>Get Pokemon By Name</summary>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(PokemonDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPokemon(string pokemonName)
        {
            string _pokemonName = HttpContext.Request.Query["pokemonName"];

            if (!await _pokemonRepository.CheckExistPokemon(_pokemonName))
                return NotFound();

            PokemonDTO pokemon = 
                await _pokemonRepository.GetPokemon(_pokemonName);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);
        }

        ///<summary>Get Pokemon Rating</summary>
        [HttpGet("{pokemonId}/rating")]
        [ProducesResponseType(200, Type = typeof(double))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPokemonRatingPoint(int pokemonId)
        {
            if (pokemonId == null)
                return BadRequest();

            if (!await _pokemonRepository.CheckExistPokemon(pokemonId))
                return NotFound();

            double ratingPoint = 
                await _pokemonRepository.GetPokemonRatingPoint(pokemonId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ratingPoint);
        }

        ///<summary>Get String Contains Pokemon Id And Name</summary>
        [HttpGet("{pokemonId}&{pokemonName}")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemon(
            [FromRoute] int pokemonId, 
            [FromRoute] string pokemonName
         )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemonId + " " + pokemonName);
        }

        ///<summary>Create Pokemon</summary>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePokemon(
            [FromQuery] int ownerId,
            [FromQuery] int categoryId,
            [FromBody] PokemonDTO pokemonCreate
        )
        {
            if (ownerId == null || categoryId == null || pokemonCreate == null)
                return BadRequest(ModelState);

            if (await _pokemonRepository.CheckExistPokemon(pokemonCreate.Name))
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonCreate))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        ///<summary>Update Pokemon</summary>
        [HttpPut("{pokemonId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePokemon(
            int pokemonId,
            [FromBody] PokemonDTO updatePokemon
        )
        {
            if (pokemonId != updatePokemon.Id)
                return BadRequest(ModelState);

            if (!await _pokemonRepository.CheckExistPokemon(pokemonId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_pokemonRepository.UpdatePokemon(updatePokemon))
            {
                ModelState.AddModelError("", "Something went wrong when updating pokemon");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        ///<summary>Delete Pokemon</summary>
        [HttpDelete("{pokemonId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePokemon(int pokemonId)
        {
            if (pokemonId == null)
                return BadRequest();

            if (!await _pokemonRepository.CheckExistPokemon(pokemonId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _reviewRepository.DeleteReviewsOfPokemon(pokemonId))
            {
                ModelState.AddModelError("", "Something went wrong when deleting reviews");
                return BadRequest(ModelState);
            }

            if (!await _pokemonRepository.DeletePokemon(pokemonId))
            {
                ModelState.AddModelError("", "Something went wrong when deleting pokemon");
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        ///<summary>Delete List Of Pokemons By Ids</summary>
        [HttpDelete("ids")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePokemonsByIds(int[] pokemonIds)
        {
            if (pokemonIds == null || pokemonIds.Length == 0)
                return BadRequest();

            if (!await _pokemonRepository.DeletePokemons(pokemonIds))
            {
                ModelState.AddModelError("", "Error when deleting pokemons");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
