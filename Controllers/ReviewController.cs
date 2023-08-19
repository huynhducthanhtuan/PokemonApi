using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.DTOs;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Controllers
{
    [Route("api/review")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public ReviewController(
            IReviewRepository reviewRepository,
            IPokemonRepository pokemonRepository,
            IReviewerRepository reviewerRepository,
            IMapper mapper
        )
        {
            _reviewRepository = reviewRepository;
            _reviewerRepository = reviewerRepository;
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        ///<summary>Get List Of Reviews</summary>
        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviews()
        {
            IEnumerable<ReviewDTO> reviews =
                _mapper.Map<IEnumerable<ReviewDTO>>(_reviewRepository.GetReviews());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        ///<summary>Get List Of Reviews By Ids</summary>
        [HttpPost("list/ids")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult GetReviewsByIds(int[] reviewIds)
        {
            IEnumerable<ReviewDTO> reviews =
                _mapper.Map<IEnumerable<ReviewDTO>>(_reviewRepository.GetReviewsByIds(reviewIds));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        ///<summary>Get Review By Id</summary>
        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(ReviewDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.CheckExistReview(reviewId))
                return NotFound();

            ReviewDTO review =
                _mapper.Map<ReviewDTO>(_reviewRepository.GetReview(reviewId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(review);
        }

        ///<summary>Get List Of Reviews Of Pokemon</summary>
        [HttpGet("pokemon/{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsOfPokemon(int pokemonId)
        {
            IEnumerable<ReviewDTO> reviews =
                _mapper.Map<IEnumerable<ReviewDTO>>(_reviewRepository.GetReviewsOfPokemon(pokemonId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(reviews);
        }

        ///<summary>Create Review</summary>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateReview(
            [FromQuery] int reviewerId,
            [FromQuery] int pokemonId,
            [FromBody] ReviewDTO reviewCreate
        )
        {
            if (reviewCreate == null)
                return BadRequest(ModelState);

            Review review = _reviewRepository.GetReviews()
                .Where(c => c.Title.Trim().ToUpper() == reviewCreate.Title.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (review != null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Review reviewMap = _mapper.Map<Review>(reviewCreate);
            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokemonId);
            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);

            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        ///<summary>Update Review</summary>
        [HttpPut("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDTO updateReview)
        {
            if (updateReview == null)
                return BadRequest(ModelState);

            if (reviewId != updateReview.Id)
                return BadRequest(ModelState);

            if (!_reviewRepository.CheckExistReview(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            Review reviewMap = _mapper.Map<Review>(updateReview);

            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong when updating review");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        ///<summary>Delete Review</summary>
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.CheckExistReview(reviewId))
                return NotFound();

            Review reviewToDelete = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", "Something went wrong when deleting owner");
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        ///<summary>Delete List Of Reviews Of Reviewer</summary>
        [HttpDelete("reviewer/{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteReviewsOfReviewer(int reviewerId)
        {
            if (!_reviewerRepository.CheckExistReviewer(reviewerId))
                return NotFound();

            IEnumerable<Review> reviewsToDelete = _reviewerRepository.GetReviewsOfReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_reviewRepository.DeleteReviews(reviewsToDelete))
            {
                ModelState.AddModelError("", "error deleting reviews");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        ///<summary>Delete List Of Reviews By Ids</summary>
        [HttpDelete("ids")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteReviewsByIds(int[] reviewIds)
        {
            IEnumerable<Review> reviews = _reviewRepository.GetReviewsByIds(reviewIds);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_reviewRepository.DeleteReviews(reviews))
            {
                ModelState.AddModelError("", "error deleting reviews");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
