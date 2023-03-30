using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Controllers
{
    [Route("api/country")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        ///<summary>Get Country List</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>
                (_countryRepository.GetCountries());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countries);
        }

        ///<summary>Get Country By Id</summary>
        [HttpGet("{countryId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Country))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            var country = _mapper.Map<CountryDto>
                (_countryRepository.GetCountry(countryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }

        ///<summary>Get Country Of An Owner</summary>
        [HttpGet("owners/{ownerId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Country))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetCountryOfAnOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>
                (_countryRepository.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(country);
        }
    }
}
