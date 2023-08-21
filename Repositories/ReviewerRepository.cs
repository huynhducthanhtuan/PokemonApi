using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.DTOs;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ReviewerRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckExistReviewer(int reviewerId)
        {
            return await _context.Reviewers.AnyAsync(r => r.Id == reviewerId);
        }

        public async Task<bool> CheckExistReviewer(string reviewerFirstName, string reviewerLastName)
        {
            return await _context.Reviewers
                .AnyAsync(r => r.FirstName == reviewerFirstName || r.LastName == reviewerLastName);
        }

        public async Task<IEnumerable<ReviewerDTO>> GetReviewers()
        {
            IEnumerable<Reviewer> reviewers = await _context.Reviewers.ToListAsync();
            IEnumerable<ReviewerDTO> reviewerDTOs = _mapper.Map<IEnumerable<ReviewerDTO>>(reviewers);
            return reviewerDTOs;
        }

        public async Task<IEnumerable<ReviewerDTO>> GetReviewersByIds(int[] reviewerIds)
        {
            IEnumerable<Reviewer> reviewers = await _context.Reviewers
                .Where(r => reviewerIds.Contains(r.Id))
                .ToListAsync();
            IEnumerable<ReviewerDTO> reviewerDTOs = 
                _mapper.Map<IEnumerable<ReviewerDTO>>(reviewers);
            return reviewerDTOs;
        }

        public async Task<ReviewerDTO> GetReviewer(int reviewerId)
        {
            Reviewer reviewer = await _context.Reviewers
                .Where(r => r.Id == reviewerId)
                .Include(e => e.Reviews)
                .FirstOrDefaultAsync();
            ReviewerDTO reviewerDTO =
                _mapper.Map<ReviewerDTO>(reviewer);
            return reviewerDTO;
        }

        public async Task<IEnumerable<ReviewDTO>> GetReviewsOfReviewer(int reviewerId)
        {
            IEnumerable<Review> reviews = await _context.Reviews
                .Where(r => r.Reviewer.Id == reviewerId)
                .ToListAsync();
            IEnumerable<ReviewDTO> reviewDTOs =
                _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
            return reviewDTOs;
        }

        public async Task<bool> CreateReviewer(ReviewerDTO reviewer)
        {
            Reviewer reviewerToCreate = _mapper.Map<Reviewer>(reviewer);
            await _context.AddAsync(reviewerToCreate);
            return Save();
        }

        public bool UpdateReviewer(ReviewerDTO reviewer)
        {
            Reviewer reviewerToUpdate = _mapper.Map<Reviewer>(reviewer);
            _context.Update(reviewerToUpdate);
            return Save();
        }

        public async Task<bool> DeleteReviewer(int reviewerId)
        {
            ReviewerDTO reviewer = await GetReviewer(reviewerId);
            Reviewer reviewerToDelete = _mapper.Map<Reviewer>(reviewer);
            _context.Remove(reviewerToDelete);
            return Save();
        }

        public async Task<bool> DeleteReviewers(int[] reviewerIds)
        {
            IEnumerable<ReviewerDTO> reviewers = await GetReviewersByIds(reviewerIds);
            IEnumerable<Reviewer> reviewersToDelete =
                _mapper.Map<IEnumerable<Reviewer>>(reviewers);

            foreach (Reviewer reviewer in reviewersToDelete)
            {
                _context.Remove(reviewer);
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
