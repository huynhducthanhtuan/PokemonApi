﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Controllers
{
    [Route("api/pokemon")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        ///<summary>Get Pokemon List</summary>
        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(pokemons);
        }

        ///<summary>Get Pokemon By Id</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Pokemon))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPokemon(int id)
        {
            if (!_pokemonRepository.PokemonExists(id))
                return NotFound();

            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(pokemon);
        }

        ///<summary>Get Pokemon By Name</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Pokemon))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPokemon(string name)
        {
            string pokemonName = HttpContext.Request.Query["name"];

            if (!_pokemonRepository.PokemonExists(pokemonName))
                return NotFound();

            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokemonName));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(pokemon);
        }

        ///<summary>Get Pokemon Rating</summary>
        [HttpGet("{pokemonId}/rating")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(double))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPokemonRating(int pokemonId)
        {
            if (!_pokemonRepository.PokemonExists(pokemonId))
                return NotFound();

            var rating = _pokemonRepository.GetPokemonRating(pokemonId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(rating);
        }

        ///<summary>Test Pass 2 Params In 1</summary>
        [HttpGet("{pokemonId}&{pokemonName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult GetPokemon([FromRoute] int pokemonId, [FromRoute] string pokemonName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
                return Ok(pokemonId + " " + pokemonName);
        }
    }
}
