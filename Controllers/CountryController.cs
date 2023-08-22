using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.DTOs;
using PokemonApi.Interfaces;

namespace PokemonApi.Controllers
{
    [Route("api/country")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;

        public CountryController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        ///<summary>Get List Of Countries</summary>
        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCountries()
        {
            IEnumerable<CountryDTO> countries = 
                await _countryRepository.GetCountryDTOs();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countries);
        }

        ///<summary>Get Country By Id</summary>
        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(CountryDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCountry(int countryId)
        {
            if (countryId == null)
                return BadRequest();

            if (!await _countryRepository.CheckExistCountry(countryId))
                return NotFound();

            CountryDTO country = 
                await _countryRepository.GetCountryDTO(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }

        ///<summary>Get Country Of Owner</summary>
        [HttpGet("owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(CountryDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCountryOfOwner(int ownerId)
        {
            if (ownerId == null)
                return BadRequest();

            CountryDTO country = 
                await _countryRepository.GetCountryDTOByOwner(ownerId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(country);
        }

        ///<summary>Create Country</summary>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateCountry(
            [FromBody] CountryDTO countryCreate
        )
        {
            if (countryCreate == null)
                return BadRequest(ModelState);

            CountryDTO country = 
                await _countryRepository.GetCountryDTO(countryCreate.Name);

            if (country != null)
            {
                ModelState.AddModelError("error", "Country already exists");
                return Conflict(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepository.CreateCountry(countryCreate))
            {
                ModelState.AddModelError("error", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction("GetCountry", new { countryId = countryCreate.Id });
        }

        ///<summary>Update Country</summary>
        [HttpPut("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateCountry(
            int countryId, 
            [FromBody] CountryDTO updateCountry
        )
        {
            if (countryId == null || updateCountry == null)
                return BadRequest(ModelState);

            if (countryId != updateCountry.Id)
                return BadRequest(ModelState);

            if (!await _countryRepository.CheckExistCountry(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_countryRepository.UpdateCountry(updateCountry))
            {
                ModelState.AddModelError("error", "Something went wrong when updating category");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        ///<summary>Delete Country</summary>
        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteCountry(int countryId)
        {
            if (countryId == null)
                return BadRequest();

            if (!await _countryRepository.CheckExistCountry(countryId))
                return NotFound();

            if (!await _countryRepository.DeleteCountry(countryId))
            {
                ModelState.AddModelError("error", "Something went wrong when deleting country");
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
