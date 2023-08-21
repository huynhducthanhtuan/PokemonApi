using PokemonApi.DTOs;
using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface IReviewerRepository
    {
        Task<bool> CheckExistReviewer(int reviewerId);
        Task<bool> CheckExistReviewer(string reviewerFirstName, string reviewerLastName);
        Task<IEnumerable<Reviewer>> GetReviewers();
        Task<IEnumerable<ReviewerDTO>> GetReviewerDTOs();
        Task<IEnumerable<Reviewer>> GetReviewersByIds(int[] reviewerIds);
        Task<IEnumerable<ReviewerDTO>> GetReviewerDTOsByIds(int[] reviewerIds);
        Task<Reviewer> GetReviewer(int reviewerId);
        Task<ReviewerDTO> GetReviewerDTO(int reviewerId);
        Task<IEnumerable<Review>> GetReviewsOfReviewer(int reviewerId);
        Task<IEnumerable<ReviewDTO>> GetReviewDTOsOfReviewer(int reviewerId);
        Task<bool> CreateReviewer(ReviewerDTO reviewer);
        bool UpdateReviewer(ReviewerDTO reviewer);
        Task<bool> DeleteReviewer(int reviewerId);
        Task<bool> DeleteReviewers(int[] reviewerIds);
        bool Save();
    }
}
