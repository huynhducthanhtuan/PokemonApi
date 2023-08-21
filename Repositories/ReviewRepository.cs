using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.DTOs;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewRepository(
            DataContext context, 
            IMapper mapper,
            IPokemonRepository pokemonRepository,
            IReviewerRepository reviewerRepository
        )
        {
            _context = context;
            _mapper = mapper;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
        }

        public async Task<bool> CheckExistReview(int reviewId)
        {
            return await _context.Reviews.AnyAsync(r => r.Id == reviewId);
        }

        public async Task<bool> CheckExistReview(string reviewTitle)
        {
            return await _context.Reviews.AnyAsync(r => r.Title == reviewTitle);
        }

        public async Task<IEnumerable<ReviewDTO>> GetReviews()
        {
            IEnumerable<Review> reviews = await _context.Reviews.ToListAsync();
            IEnumerable<ReviewDTO> reviewDTOs = _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
            return reviewDTOs;
        }

        public async Task<IEnumerable<ReviewDTO>> GetReviewsByIds(int[] reviewIds)
        {
            IEnumerable<Review> reviews = await _context.Reviews
                .Where(r => reviewIds.Contains(r.Id))
                .ToListAsync();
            IEnumerable<ReviewDTO> reviewDTOs = 
                _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
            return reviewDTOs;
        }

        public async Task<IEnumerable<ReviewDTO>> GetReviewsOfPokemon(int pokemonId)
        {
            IEnumerable<Review> reviews = await _context.Reviews
                .Where(r => r.Pokemon.Id == pokemonId)
                .ToListAsync();
            IEnumerable<ReviewDTO> reviewDTOs =
                _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
            return reviewDTOs;
        }

        public async Task<ReviewDTO> GetReview(int reviewId)
        {
            Review review = await _context.Reviews
                .Where(r => r.Id == reviewId)
                .FirstOrDefaultAsync();
            ReviewDTO reviewDTO = _mapper.Map<ReviewDTO>(review);
            return reviewDTO;
        }

        public async Task<bool> CreateReview(
            int reviewerId,
            int pokemonId,
            ReviewDTO review
        )
        {
            Review reviewToCreate = _mapper.Map<Review>(review);
            reviewToCreate.Pokemon = 
                _mapper.Map<Pokemon>(await _pokemonRepository.GetPokemon(pokemonId));
            reviewToCreate.Reviewer =
                _mapper.Map<Reviewer>(await _reviewerRepository.GetReviewer(reviewerId));

            await _context.AddAsync(reviewToCreate);
            return Save();
        }

        public bool UpdateReview(ReviewDTO review)
        {
            Review reviewToUpdate = _mapper.Map<Review>(review);
            _context.Update(reviewToUpdate);
            return Save();
        }

        public async Task<bool> DeleteReview(int reviewId)
        {
            ReviewDTO review = await GetReview(reviewId);
            Review reviewToDelete = _mapper.Map<Review>(review);
            _context.Remove(reviewToDelete);
            return Save();
        }

        public async Task<bool> DeleteReviews(int[] reviewIds)
        {
            IEnumerable<ReviewDTO> reviews = await GetReviewsByIds(reviewIds);
            IEnumerable<Review> reviewsToDelete = 
                _mapper.Map<IEnumerable<Review>>(reviews);

            foreach (Review review in reviewsToDelete)
            {
                _context.Remove(review);
            }
            return Save();
        }

        public async Task<bool> DeleteReviewsOfPokemon(int pokemonId)
        {
            IEnumerable<Review> reviewsToDelete = await _context.Reviews
                .Where(r => r.Pokemon.Id == pokemonId)
                .ToListAsync();

            foreach (Review review in reviewsToDelete)
            {
                _context.Remove(review);
            }
            return Save();
        }

        public async Task<bool> DeleteReviewsOfReviewer(int reviewerId)
        {
            IEnumerable<Review> reviewsToDelete = await _context.Reviews
                .Where(r => r.Reviewer.Id == reviewerId)
                .ToListAsync();

            foreach (Review review in reviewsToDelete)
            {
                _context.Remove(review);
            }
            return Save();
        }

        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
