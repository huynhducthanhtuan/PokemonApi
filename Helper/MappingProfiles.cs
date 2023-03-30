using AutoMapper;
using PokemonApi.Dto;
using PokemonApi.Models;

namespace PokemonApi.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Country, CountryDto>();
            CreateMap<Pokemon, PokemonDto>();
        }
    }
}
