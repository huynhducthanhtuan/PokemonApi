using Microsoft.AspNetCore.Mvc;
using PokemonApi.DTOs;
using PokemonApi.Interfaces;

namespace PokemonApi.Controllers
{
    [Route("api/review")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewController(
            IReviewRepository reviewRepository,
            IReviewerRepository reviewerRepository
        )
        {
            _reviewRepository = reviewRepository;
            _reviewerRepository = reviewerRepository;
        }

        ///<summary>Get List Of Reviews</summary>
        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDTO>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviews()
        {
            IEnumerable<ReviewDTO> reviews = await _reviewRepository.GetReviews();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        ///<summary>Get List Of Reviews By Ids</summary>
        [HttpPost("list/ids")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetReviewsByIds(int[] reviewIds)
        {
            if (reviewIds == null || reviewIds.Length == 0)
                return BadRequest(ModelState);

            IEnumerable<ReviewDTO> reviews = 
                await _reviewRepository.GetReviewsByIds(reviewIds);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        ///<summary>Get Review By Id</summary>
        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(ReviewDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetReview(int reviewId)
        {
            if (reviewId == null)
                return BadRequest(ModelState);

            if (!await _reviewRepository.CheckExistReview(reviewId))
                return NotFound();

            ReviewDTO review = await _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(review);
        }

        ///<summary>Get List Of Reviews Of Pokemon</summary>
        [HttpGet("pokemon/{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDTO>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewsOfPokemon(int pokemonId)
        {
            if (pokemonId == null)
                return BadRequest(ModelState);

            IEnumerable<ReviewDTO> reviews =
                await _reviewRepository.GetReviewsOfPokemon(pokemonId);

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
        public async Task<IActionResult> CreateReview(
            [FromQuery] int reviewerId,
            [FromQuery] int pokemonId,
            [FromBody] ReviewDTO reviewCreate
        )
        {
            if (reviewerId == null || pokemonId == null || reviewCreate == null)
                return BadRequest(ModelState);

            if (reviewCreate == null)
                return BadRequest(ModelState);

            if (await _reviewRepository.CheckExistReview(reviewCreate.Title))
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _reviewRepository.CreateReview(reviewerId, pokemonId, reviewCreate))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
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
        public async Task<IActionResult> UpdateReview(
            int reviewId, 
            [FromBody] ReviewDTO updateReview)
        {
            if (reviewId == null || updateReview == null)
                return BadRequest(ModelState);

            if (reviewId != updateReview.Id)
                return BadRequest(ModelState);

            if (!await _reviewRepository.CheckExistReview(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_reviewRepository.UpdateReview(updateReview))
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
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            if (reviewId == null)
                return BadRequest(ModelState);

            if (!await _reviewRepository.CheckExistReview(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _reviewRepository.DeleteReview(reviewId))
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
        public async Task<IActionResult> DeleteReviewsOfReviewer(int reviewerId)
        {
            if (reviewerId == null)
                return BadRequest();

            if (!await _reviewerRepository.CheckExistReviewer(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            if (!await _reviewRepository.DeleteReviewsOfReviewer(reviewerId))
            {
                ModelState.AddModelError("", "Error when deleting reviews");
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
        public async Task<IActionResult> DeleteReviewsByIds(int[] reviewIds)
        {
            if (reviewIds == null || reviewIds.Length == 0)
                return BadRequest();

            if (!await _reviewRepository.DeleteReviews(reviewIds))
            {
                ModelState.AddModelError("", "Error when deleting reviews");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
