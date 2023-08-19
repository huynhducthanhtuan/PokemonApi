using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface IReviewRepository
    {
        bool CheckExistReview(int reviewId);
        ICollection<Review> GetReviews();
        ICollection<Review> GetReviewsByIds(int[] reviewIds);
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsOfAPokemon(int pokemonId);
        bool CreateReview(Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool DeleteReviews(List<Review> reviews);
        bool Save();
    }
}
