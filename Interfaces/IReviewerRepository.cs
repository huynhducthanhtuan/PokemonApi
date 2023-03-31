using PokemonApi.Models;

namespace PokemonApi.Interfaces
{
    public interface IReviewerRepository
    {
        bool ReviewerExists(int reviewerId);
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int reviewerId);
        ICollection<Review> GetReviewsByReviewer(int reviewerId);
        bool CreateReviewer(Reviewer reviewer);
        bool UpdateReviewer(Reviewer reviewer);
        bool DeleteReviewer(Reviewer reviewer);
        // bool DeleteReviewers(List<Reviewer> reviewers);
        bool Save();
    }
}
