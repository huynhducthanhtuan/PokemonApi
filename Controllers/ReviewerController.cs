using Microsoft.AspNetCore.Mvc;
using PokemonApi.DTOs;
using PokemonApi.Interfaces;

namespace PokemonApi.Controllers
{
    [Route("api/reviewer")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewerController(IReviewerRepository reviewerRepository)
        {
            _reviewerRepository = reviewerRepository;
        }

        ///<summary>Get List Of Reviewers</summary>
        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDTO>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewers()
        {
            IEnumerable<ReviewerDTO> reviewers =
                await _reviewerRepository.GetReviewerDTOs();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewers);
        }

        ///<summary>Get List Of Reviewers By Ids</summary>
        [HttpPost("list/ids")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetReviewersByIds(int[] reviewerIds)
        {
            if (reviewerIds == null || reviewerIds.Length == 0)
                return BadRequest(ModelState);

            IEnumerable<ReviewerDTO> reviewers =
                await _reviewerRepository.GetReviewerDTOsByIds(reviewerIds);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewers);
        }

        ///<summary>Get Reviewer By Id</summary>
        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(ReviewerDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPokemon(int reviewerId)
        {
            if (reviewerId == null)
                return BadRequest(ModelState);

            if (!await _reviewerRepository.CheckExistReviewer(reviewerId))
                return NotFound();

            ReviewerDTO reviewer =
                await _reviewerRepository.GetReviewerDTO(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewer);
        }

        ///<summary>Get List Of Reviews Of Reviewer</summary>
        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetReviewsOfReviewer(int reviewerId)
        {
            if (reviewerId == null)
                return BadRequest(ModelState);

            if (!await _reviewerRepository.CheckExistReviewer(reviewerId))
                return NotFound();

            IEnumerable<ReviewDTO> reviews =
                await _reviewerRepository.GetReviewDTOsOfReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        ///<summary>Create Reviewer</summary>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateReviewer(
            [FromBody] ReviewerDTO reviewerCreate
        )
        {
            if (reviewerCreate == null)
                return BadRequest(ModelState);

            bool isExistReviewer =  await _reviewerRepository.CheckExistReviewer(
                reviewerCreate.FirstName, 
                reviewerCreate.LastName
            );

            if (isExistReviewer == true)
            {
                ModelState.AddModelError("", "Reviewer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _reviewerRepository.CreateReviewer(reviewerCreate))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        ///<summary>Update Reviewer</summary>
        [HttpPut("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateReviewer(
            int reviewerId, 
            [FromBody] ReviewerDTO updateReviewer
        )
        {
            if (reviewerId == null || updateReviewer == null)
                return BadRequest(ModelState);

            if (reviewerId != updateReviewer.Id)
                return BadRequest(ModelState);

            if (!await _reviewerRepository.CheckExistReviewer(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_reviewerRepository.UpdateReviewer(updateReviewer))
            {
                ModelState.AddModelError("", "Something went wrong when updating reviewer");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        ///<summary>Delete Reviewer</summary>
        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteReviewer(int reviewerId)
        {
            if (reviewerId == null)
                return BadRequest(ModelState);

            if (!await _reviewerRepository.CheckExistReviewer(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _reviewerRepository.CheckExistReviewer(reviewerId))
            {
                ModelState.AddModelError("", "Something went wrong when deleting reviewer");
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        ///<summary>Delete List Of Reviewers By Ids</summary>
        [HttpDelete("ids")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteReviewersByIds(int[] reviewerIds)
        {
            if (reviewerIds == null || reviewerIds.Length == 0)
                return BadRequest(ModelState);

            if (!await _reviewerRepository.DeleteReviewers(reviewerIds))
            {
                ModelState.AddModelError("", "Error when deleting reviewers");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
