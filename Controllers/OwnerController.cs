using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.DTOs;
using PokemonApi.Interfaces;
using PokemonApi.Models;
using System.Collections.Generic;

namespace PokemonApi.Controllers
{
    [Route("api/owner")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public OwnerController(
            IOwnerRepository ownerRepository,
            ICountryRepository countryRepository,
            IMapper mapper
        )
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        ///<summary>Get List Of Owners</summary>
        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OwnerDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwners()
        {
            IEnumerable<OwnerDTO> owners =
                _mapper.Map<IEnumerable<OwnerDTO>>(_ownerRepository.GetOwners());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);
        }

        ///<summary>Get List Of Owners By Ids</summary>
        [HttpPost("list/ids")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OwnerDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult GetOwnersByIds(int[] ownerIds)
        {
            IEnumerable<OwnerDTO> owners =
                _mapper.Map<IEnumerable<OwnerDTO>>(_ownerRepository.GetOwnersByIds(ownerIds));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);
        }

        ///<summary>Get Owner By Id</summary>
        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(OwnerDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetOwner(int ownerId)
        {
            if (!_ownerRepository.CheckExistOwner(ownerId))
                return NotFound();

            OwnerDTO owner =
                _mapper.Map<OwnerDTO>(_ownerRepository.GetOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }

        ///<summary>Get Pokemon By Owner Id</summary>
        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if (!_ownerRepository.CheckExistOwner(ownerId))
                return NotFound();

            IEnumerable<PokemonDTO> owner =
                _mapper.Map<IEnumerable<PokemonDTO>>(_ownerRepository.GetPokemonsByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }

        ///<summary>Create Owner</summary>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDTO ownerCreate)
        {
            if (ownerCreate == null)
                return BadRequest(ModelState);

            Owner owner = _ownerRepository.GetOwners()
                .Where(c => c.LastName.Trim().ToUpper() == ownerCreate.LastName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (owner != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Owner ownerMap = _mapper.Map<Owner>(ownerCreate);
            ownerMap.Country = _countryRepository.GetCountry(countryId);

            if (!_ownerRepository.CreateOwner(ownerMap))
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
        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDTO updateOwner)
        {
            if (updateOwner == null)
                return BadRequest(ModelState);

            if (ownerId != updateOwner.Id)
                return BadRequest(ModelState);

            if (!_ownerRepository.CheckExistOwner(ownerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            Owner ownerMap = _mapper.Map<Owner>(updateOwner);

            if (!_ownerRepository.UpdateOwner(ownerMap))
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
        public IActionResult DeleteOwner(int ownerId)
        {
            if (!_ownerRepository.CheckExistOwner(ownerId))
                return NotFound();

            Owner ownerToDelete = _ownerRepository.GetOwner(ownerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_ownerRepository.DeleteOwner(ownerToDelete))
                ModelState.AddModelError("", "Something went wrong deleting owner");

            return NoContent();
        }

        ///<summary>Delete List Of Owners By Ids</summary>
        [HttpDelete("ids")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult DeleteOwnersByIds(int[] ownerIds)
        {
            IEnumerable<Owner> ownersToDelete = _ownerRepository.GetOwnersByIds(ownerIds);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_ownerRepository.DeleteOwners(ownersToDelete))
            {
                ModelState.AddModelError("", "Error when deleting owners");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
