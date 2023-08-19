using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface IReviewRepository
    {
        bool CheckExistReview(int reviewId);
        IEnumerable<Review> GetReviews();
        IEnumerable<Review> GetReviewsByIds(int[] reviewIds);
        Review GetReview(int reviewId);
        IEnumerable<Review> GetReviewsOfPokemon(int pokemonId);
        bool CreateReview(Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool DeleteReviews(IEnumerable<Review> reviews);
        bool Save();
    }
}
