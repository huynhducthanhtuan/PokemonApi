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
            CreateMap<Owner, OwnerDto>();
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<Reviewer, ReviewerDto>();
            CreateMap<Review, ReviewDto>();
        }
    }
}
