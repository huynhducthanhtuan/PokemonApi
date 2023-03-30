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

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsOfAPokemon(int pokemonId)
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

        public bool ReviewExists(int reviewId)
        {
            return _context.Reviews.Any(r => r.Id == reviewId);
        }
    }
}
