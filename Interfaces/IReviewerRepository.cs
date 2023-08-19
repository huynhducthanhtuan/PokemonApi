using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface IReviewerRepository
    {
        bool ReviewerExists(int reviewerId);
        ICollection<Reviewer> GetReviewers();
        ICollection<Reviewer> GetReviewersByIds(int[] reviewerIds);
        Reviewer GetReviewer(int reviewerId);
        ICollection<Review> GetReviewsByReviewerId(int reviewerId);
        bool CreateReviewer(Reviewer reviewer);
        bool UpdateReviewer(Reviewer reviewer);
        bool DeleteReviewer(Reviewer reviewer);
        bool DeleteReviewers(List<Reviewer> reviewers);
        bool Save();
    }
}
