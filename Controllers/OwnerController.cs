using Microsoft.AspNetCore.Mvc;
using PokemonApi.DTOs;
using PokemonApi.Interfaces;

namespace PokemonApi.Controllers
{
    [Route("api/owner")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;

        public OwnerController(
            IOwnerRepository ownerRepository,
            ICountryRepository countryRepository
        )
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
        }

        ///<summary>Get List Of Owners</summary>
        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OwnerDTO>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOwners()
        {
            IEnumerable<OwnerDTO> owners = 
                await _ownerRepository.GetOwnerDTOs();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);
        }

        ///<summary>Get List Of Owners By Ids</summary>
        [HttpPost("list/ids")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OwnerDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetOwnersByIds(int[] ownerIds)
        {
            if (ownerIds == null || ownerIds.Length == 0)
                return BadRequest();

            IEnumerable<OwnerDTO> owners = 
                await _ownerRepository.GetOwnerDTOsByIds(ownerIds);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);
        }

        ///<summary>Get Owner By Id</summary>
        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(OwnerDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetOwner(int ownerId)
        {
            if (ownerId == null)
                return BadRequest();

            if (!await _ownerRepository.CheckExistOwner(ownerId))
                return NotFound();

            OwnerDTO owner = 
                await _ownerRepository.GetOwnerDTO(ownerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }

        ///<summary>Get Pokemon By Owner Id</summary>
        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPokemonsByOwner(int ownerId)
        {
            if (ownerId == null)
                return BadRequest();

            if (!await _ownerRepository.CheckExistOwner(ownerId))
                return NotFound();

            IEnumerable<PokemonDTO> pokemons =
                await _ownerRepository.GetPokemonDTOsByOwner(ownerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        ///<summary>Create Owner</summary>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateOwner(
            [FromQuery] int countryId,
            [FromBody] OwnerDTO ownerCreate
        )
        {
            if (countryId == null || ownerCreate == null)
                return BadRequest(ModelState);

            OwnerDTO owner = await _ownerRepository.GetOwnerDTO(
                ownerCreate.FirstName, 
                ownerCreate.LastName
            );

            if (owner != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            CountryDTO country = await _countryRepository.GetCountryDTO(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _ownerRepository.CreateOwner(owner, country))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        ///<summary>Update Owner</summary>
        [HttpPut("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateOwner(
            int ownerId, 
            [FromBody] OwnerDTO updateOwner
        )
        {
            if (ownerId == null || updateOwner == null)
                return BadRequest(ModelState);

            if (ownerId != updateOwner.Id)
                return BadRequest(ModelState);

            if (!await _ownerRepository.CheckExistOwner(ownerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_ownerRepository.UpdateOwner(updateOwner))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        ///<summary>Delete Owner</summary>
        [HttpDelete("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteOwner(int ownerId)
        {
            if (ownerId == null)
                return BadRequest();

            if (!await _ownerRepository.CheckExistOwner(ownerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _ownerRepository.DeleteOwner(ownerId))
                ModelState.AddModelError("", "Something went wrong when deleting owner");

            return NoContent();
        }

        ///<summary>Delete List Of Owners By Ids</summary>
        [HttpDelete("ids")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteOwnersByIds(int[] ownerIds)
        {
            if (ownerIds == null || ownerIds.Length == 0)
                return BadRequest();

            if (!await _ownerRepository.DeleteOwners(ownerIds))
            {
                ModelState.AddModelError("", "Error when deleting owners");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
