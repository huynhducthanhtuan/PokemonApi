using PokemonApi.DTOs;

namespace PokemonApi.Interfaces
{
    public interface IReviewerRepository
    {
        Task<bool> CheckExistReviewer(int reviewerId);
        Task<bool> CheckExistReviewer(string reviewerFirstName, string reviewerLastName);
        Task<IEnumerable<ReviewerDTO>> GetReviewers();
        Task<IEnumerable<ReviewerDTO>> GetReviewersByIds(int[] reviewerIds);
        Task<ReviewerDTO> GetReviewer(int reviewerId);
        Task<IEnumerable<ReviewDTO>> GetReviewsOfReviewer(int reviewerId);
        Task<bool> CreateReviewer(ReviewerDTO reviewer);
        bool UpdateReviewer(ReviewerDTO reviewer);
        Task<bool> DeleteReviewer(int reviewerId);
        Task<bool> DeleteReviewers(int[] reviewerIds);
        bool Save();
    }
}
