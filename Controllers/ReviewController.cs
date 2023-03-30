using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Controllers
{
    [Route("api/review")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        ///<summary>Get Review List</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>
                (_reviewRepository.GetReviews());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        ///<summary>Get Pokemon By Review Id</summary>
        [HttpGet("{reviewId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Review))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPokemon(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var review = _mapper.Map<ReviewDto>
                (_reviewRepository.GetReview(reviewId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(review);
        }

        ///<summary>Get Reviews For A Pokemon</summary>
        [HttpGet("pokemon/{pokemonId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Review))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetReviewsForAPokemon(int pokemonId)
        {
            var reviews = _mapper.Map<List<ReviewDto>>
                (_reviewRepository.GetReviewsOfAPokemon(pokemonId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(reviews);
        }
    }
}
