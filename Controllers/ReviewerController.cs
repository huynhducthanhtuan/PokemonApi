using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.DTOs;
using PokemonApi.Interfaces;
using PokemonApi.Models;
using System.Collections.Generic;

namespace PokemonApi.Controllers
{
    [Route("api/reviewer")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        ///<summary>Get List Of Reviewers</summary>
        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewers()
        {
            IEnumerable<ReviewerDTO> reviewers =
                _mapper.Map<IEnumerable<ReviewerDTO>>(_reviewerRepository.GetReviewers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewers);
        }

        ///<summary>Get List Of Reviewers By Ids</summary>
        [HttpPost("list/ids")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult GetReviewersByIds(int[] reviewerIds)
        {
            IEnumerable<ReviewerDTO> reviewers =
                _mapper.Map<IEnumerable<ReviewerDTO>>(_reviewerRepository.GetReviewersByIds(reviewerIds));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewers);
        }

        ///<summary>Get Reviewer By Id</summary>
        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(ReviewerDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemon(int reviewerId)
        {
            if (!_reviewerRepository.CheckExistReviewer(reviewerId))
                return NotFound();

            ReviewerDTO reviewer =
                _mapper.Map<ReviewerDTO>(_reviewerRepository.GetReviewer(reviewerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewer);
        }

        ///<summary>Get List Of Reviews Of Reviewer</summary>
        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewsOfReviewer(int reviewerId)
        {
            if (!_reviewerRepository.CheckExistReviewer(reviewerId))
                return NotFound();

            IEnumerable<ReviewDTO> reviews =
                _mapper.Map<IEnumerable<ReviewDTO>>(_reviewerRepository.GetReviewsOfReviewer(reviewerId));

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
        public IActionResult CreateReviewer([FromBody] ReviewerDTO reviewerCreate)
        {
            if (reviewerCreate == null)
                return BadRequest(ModelState);

            Reviewer reviewer = _reviewerRepository.GetReviewers()
                .Where(c => c.LastName.Trim().ToUpper() == reviewerCreate.LastName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (reviewer != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Reviewer reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);

            if (!_reviewerRepository.CreateReviewer(reviewerMap))
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
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDTO updateReviewer)
        {
            if (updateReviewer == null)
                return BadRequest(ModelState);

            if (reviewerId != updateReviewer.Id)
                return BadRequest(ModelState);

            if (!_reviewerRepository.CheckExistReviewer(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            Reviewer reviewerMap = _mapper.Map<Reviewer>(updateReviewer);

            if (!_reviewerRepository.UpdateReviewer(reviewerMap))
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
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.CheckExistReviewer(reviewerId))
                return NotFound();

            Reviewer reviewerToDelete = _reviewerRepository.GetReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepository.DeleteReviewer(reviewerToDelete))
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
        public IActionResult DeleteReviewersByIds(int[] reviewerIds)
        {
            IEnumerable<Reviewer> reviewers = _reviewerRepository.GetReviewersByIds(reviewerIds);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_reviewerRepository.DeleteReviewers(reviewers))
            {
                ModelState.AddModelError("", "Error when deleting reviewers");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
