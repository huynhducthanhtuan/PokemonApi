using PokemonApi.DTOs;
using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface IReviewRepository
    {
        Task<bool> CheckExistReview(int reviewId);
        Task<bool> CheckExistReview(string reviewTitle);
        Task<IEnumerable<Review>> GetReviews();
        Task<IEnumerable<ReviewDTO>> GetReviewDTOs();
        Task<IEnumerable<Review>> GetReviewsByIds(int[] reviewIds);
        Task<IEnumerable<ReviewDTO>> GetReviewDTOsByIds(int[] reviewIds);
        Task<Review> GetReview(int reviewId);
        Task<ReviewDTO> GetReviewDTO(int reviewId);
        Task<IEnumerable<Review>> GetReviewsOfPokemon(int pokemonId);
        Task<IEnumerable<ReviewDTO>> GetReviewDTOsOfPokemon(int pokemonId);
        Task<bool> CreateReview(int reviewerId, int pokemonId, ReviewDTO review);
        bool UpdateReview(ReviewDTO review);
        Task<bool> DeleteReview(int reviewId);
        Task<bool> DeleteReviews(int[] reviewIds);
        Task<bool> DeleteReviewsOfPokemon(int pokemonId);
        Task<bool> DeleteReviewsOfReviewer(int reviewerId);
        bool Save();
    }
}
