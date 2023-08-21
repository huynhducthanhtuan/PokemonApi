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
                .AnyAsync(r => r.FirstName == reviewerFirstName 
                            || r.LastName == reviewerLastName);
        }

        public async Task<IEnumerable<Reviewer>> GetReviewers()
        {
            return await _context.Reviewers.ToListAsync();
        }

        public async Task<IEnumerable<ReviewerDTO>> GetReviewerDTOs()
        {
            IEnumerable<Reviewer> reviewers = await GetReviewers();
            IEnumerable<ReviewerDTO> reviewerDTOs = _mapper.Map<IEnumerable<ReviewerDTO>>(reviewers);
            return reviewerDTOs;
        }

        public async Task<IEnumerable<Reviewer>> GetReviewersByIds(int[] reviewerIds)
        {
            return await _context.Reviewers
                .Where(r => reviewerIds.Contains(r.Id))
                .ToListAsync();
        }

        public async Task<IEnumerable<ReviewerDTO>> GetReviewerDTOsByIds(int[] reviewerIds)
        {
            IEnumerable<Reviewer> reviewers = await GetReviewersByIds(reviewerIds);
            IEnumerable<ReviewerDTO> reviewerDTOs = _mapper.Map<IEnumerable<ReviewerDTO>>(reviewers);
            return reviewerDTOs;
        }

        public async Task<Reviewer> GetReviewer(int reviewerId)
        {
            return await _context.Reviewers
                .Where(r => r.Id == reviewerId)
                .Include(e => e.Reviews)
                .FirstOrDefaultAsync();
        }

        public async Task<ReviewerDTO> GetReviewerDTO(int reviewerId)
        {
            Reviewer reviewer = await GetReviewer(reviewerId);
            ReviewerDTO reviewerDTO = _mapper.Map<ReviewerDTO>(reviewer);
            return reviewerDTO;
        }

        public async Task<IEnumerable<Review>> GetReviewsOfReviewer(int reviewerId)
        {
            return await _context.Reviews
                .Where(r => r.Reviewer.Id == reviewerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ReviewDTO>> GetReviewDTOsOfReviewer(int reviewerId)
        {
            IEnumerable<Review> reviews = await GetReviewsOfReviewer(reviewerId);
            IEnumerable<ReviewDTO> reviewDTOs = _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
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
            Reviewer reviewerToDelete = await GetReviewer(reviewerId);
            _context.Remove(reviewerToDelete);
            return Save();
        }

        public async Task<bool> DeleteReviewers(int[] reviewerIds)
        {
            IEnumerable<Reviewer> reviewersToDelete = await GetReviewersByIds(reviewerIds);
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
