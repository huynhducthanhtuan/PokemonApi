using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsOfAPokemon(int pokemonId);
        bool ReviewExists(int reviewId);
    }
}
