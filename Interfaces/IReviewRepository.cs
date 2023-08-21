using PokemonApi.DTOs;

namespace PokemonApi.Interfaces
{
    public interface IReviewRepository
    {
        Task<bool> CheckExistReview(int reviewId);
        Task<bool> CheckExistReview(string reviewTitle);
        Task<IEnumerable<ReviewDTO>> GetReviews();
        Task<IEnumerable<ReviewDTO>> GetReviewsByIds(int[] reviewIds);
        Task<ReviewDTO> GetReview(int reviewId);
        Task<IEnumerable<ReviewDTO>> GetReviewsOfPokemon(int pokemonId);
        Task<bool> CreateReview(int reviewerId, int pokemonId, ReviewDTO review);
        bool UpdateReview(ReviewDTO review);
        Task<bool> DeleteReview(int reviewId);
        Task<bool> DeleteReviews(int[] reviewIds);
        Task<bool> DeleteReviewsOfPokemon(int pokemonId);
        Task<bool> DeleteReviewsOfReviewer(int reviewerId);
        bool Save();
    }
}
