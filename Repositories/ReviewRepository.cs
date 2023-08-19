using PokemonApi.Data;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }

        public bool CheckExistReview(int reviewId)
        {
            return _context.Reviews.Any(r => r.Id == reviewId);
        }

        public IEnumerable<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public IEnumerable<Review> GetReviewsByIds(int[] reviewIds)
        {
            return _context.Reviews
                .Where(r => reviewIds.Contains(r.Id))
                .ToList();
        }

        public IEnumerable<Review> GetReviewsOfPokemon(int pokemonId)
        {
            return _context.Reviews
                .Where(r => r.Pokemon.Id == pokemonId)
                .ToList();
        }

        public Review GetReview(int reviewId)
        {
            return _context.Reviews
                .Where(r => r.Id == reviewId)
                .FirstOrDefault();
        }

        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return Save();
        }

        public bool UpdateReview(Review review)
        {
            _context.Update(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _context.Remove(review);
            return Save();
        }

        public bool DeleteReviews(IEnumerable<Review> reviews)
        {
            foreach (Review review in reviews)
            {
                _context.Remove(review);
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
