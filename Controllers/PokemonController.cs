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
        )
        {
            _pokemonRepository = pokemonRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        ///<summary>Get List Of Pokemons</summary>
        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemons()
        {
            IEnumerable<PokemonDTO> pokemons =
                _mapper.Map<IEnumerable<PokemonDTO>>(_pokemonRepository.GetPokemons());

            if (!ModelState.IsValid)
                BadRequest(ModelState);

            return Ok(pokemons);
        }

        ///<summary>Get List Of Pokemons By Ids</summary>
        [HttpPost("list/ids")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult GetPokemonsByIds(int[] pokemonIds)
        {
            IEnumerable<PokemonDTO> pokemons =
                _mapper.Map<IEnumerable<PokemonDTO>>(_pokemonRepository.GetPokemonsByIds(pokemonIds));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        ///<summary>Get Pokemon By Id</summary>
        [HttpGet("{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(PokemonDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemon(int pokemonId)
        {
            if (!_pokemonRepository.CheckExistPokemon(pokemonId))
                return NotFound();

            PokemonDTO pokemon =
                _mapper.Map<PokemonDTO>(_pokemonRepository.GetPokemon(pokemonId));

            if (!ModelState.IsValid)
                BadRequest(ModelState);

            return Ok(pokemon);
        }

        ///<summary>Get Pokemon By Name</summary>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(PokemonDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemon(string pokemonName)
        {
            string _pokemonName = HttpContext.Request.Query["pokemonName"];

            if (!_pokemonRepository.CheckExistPokemon(_pokemonName))
                return NotFound();

            PokemonDTO pokemon = _mapper.Map<PokemonDTO>
                (_pokemonRepository.GetPokemon(_pokemonName));

            if (!ModelState.IsValid)
                BadRequest(ModelState);

            return Ok(pokemon);
        }

        ///<summary>Get Pokemon Rating</summary>
        [HttpGet("{pokemonId}/rating")]
        [ProducesResponseType(200, Type = typeof(double))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemonRating(int pokemonId)
        {
            if (!_pokemonRepository.CheckExistPokemon(pokemonId))
                return NotFound();

            double rating = _pokemonRepository.GetPokemonRating(pokemonId);

            if (!ModelState.IsValid)
                BadRequest(ModelState);

            return Ok(rating);
        }

        ///<summary>Get String Contains Pokemon Id And Name</summary>
        [HttpGet("{pokemonId}&{pokemonName}")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon([FromRoute] int pokemonId, [FromRoute] string pokemonName)
        {
            if (!ModelState.IsValid)
                BadRequest(ModelState);

            return Ok(pokemonId + " " + pokemonName);
        }

        ///<summary>Create Pokemon</summary>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreatePokemon(
            [FromQuery] int ownerId,
            [FromQuery] int categoryId,
            [FromBody] PokemonDTO pokemonCreate
        )
        {
            if (pokemonCreate == null)
                return BadRequest(ModelState);

            Pokemon pokemon = _pokemonRepository.GetPokemonTrimToUpper(pokemonCreate);

            if (pokemon != null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Pokemon pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

            if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
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
        public IActionResult UpdatePokemon(
            int pokemonId,
            [FromQuery] int ownerId,
            [FromQuery] int categoryId,
            [FromBody] PokemonDTO updatePokemon
        )
        {
            if (updatePokemon == null)
                return BadRequest(ModelState);

            if (pokemonId != updatePokemon.Id)
                return BadRequest(ModelState);

            if (!_pokemonRepository.CheckExistPokemon(pokemonId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            Pokemon pokemonMap = _mapper.Map<Pokemon>(updatePokemon);

            if (!_pokemonRepository.UpdatePokemon(ownerId, categoryId, pokemonMap))
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
            if (!_pokemonRepository.CheckExistPokemon(pokemonId))
                return NotFound();

            IEnumerable<Review> reviewsToDelete = _reviewRepository.GetReviewsOfPokemon(pokemonId);
            Pokemon pokemonToDelete = _pokemonRepository.GetPokemon(pokemonId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting reviews");
                return BadRequest(ModelState);
            }

            if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting pokemon");
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
        public IActionResult DeletePokemonsByIds(int[] pokemonIds)
        {
            IEnumerable<Pokemon> pokemons = _pokemonRepository.GetPokemonsByIds(pokemonIds);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_pokemonRepository.DeletePokemons(pokemons))
            {
                ModelState.AddModelError("", "Error when deleting pokemons");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
