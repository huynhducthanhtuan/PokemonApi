using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface IReviewerRepository
    {
        bool CheckExistReviewer(int reviewerId);
        IEnumerable<Reviewer> GetReviewers();
        IEnumerable<Reviewer> GetReviewersByIds(int[] reviewerIds);
        Reviewer GetReviewer(int reviewerId);
        IEnumerable<Review> GetReviewsOfReviewer(int reviewerId);
        bool CreateReviewer(Reviewer reviewer);
        bool UpdateReviewer(Reviewer reviewer);
        bool DeleteReviewer(Reviewer reviewer);
        bool DeleteReviewers(IEnumerable<Reviewer> reviewers);
        bool Save();
    }
}
