using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _context;

        public ReviewerRepository(DataContext context)
        {
            _context = context;
        }

        public bool CheckExistReviewer(int reviewerId)
        {
            return _context.Reviewers.Any(r => r.Id == reviewerId);
        }

        public IEnumerable<Reviewer> GetReviewers()
        {
            return _context.Reviewers.ToList();
        }

        public IEnumerable<Reviewer> GetReviewersByIds(int[] reviewerIds)
        {
            return _context.Reviewers
                .Where(r => reviewerIds.Contains(r.Id))
                .ToList();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return _context.Reviewers
                .Where(r => r.Id == reviewerId)
                .Include(e => e.Reviews)
                .FirstOrDefault();
        }

        public IEnumerable<Review> GetReviewsOfReviewer(int reviewerId)
        {
            return _context.Reviews
                .Where(r => r.Reviewer.Id == reviewerId)
                .ToList();
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _context.Add(reviewer);
            return Save();
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _context.Update(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _context.Remove(reviewer);
            return Save();
        }

        public bool DeleteReviewers(IEnumerable<Reviewer> reviewers)
        {
            foreach (Reviewer reviewer in reviewers)
            {
                _context.Remove(reviewer);
            }
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
