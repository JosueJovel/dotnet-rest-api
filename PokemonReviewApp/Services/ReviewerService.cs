using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services
{
    public class ReviewerService
    {
        private readonly IMapper _mapper;
        private IReviewerRepository _reviewerRepository;
        private readonly DataContext _context;

        public ReviewerService(IMapper mapper, IReviewerRepository reviewerRepository, DataContext context)
        {
            this._mapper = mapper;
            this._reviewerRepository = reviewerRepository;
            this._context = context;
        }

        public bool ReviewerNameExists(ReviewerDto ReviewerDto)
        {
            var namedReviewer = _reviewerRepository.GetReviewers()
                .Where(c => c.FirstName.Trim().ToUpper() == ReviewerDto.FirstName.TrimEnd().ToUpper() && c.LastName.Trim().ToUpper() == ReviewerDto.LastName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (namedReviewer != null) //If a named match of the Reviewer is found/not null
            {
                return true; //ReviewerName exists, return true
            }
            else return false; //ReviewerName does not exist, return false
        }

        public bool SaveReviewerToDb(ReviewerDto requestReviewerDto)
        {
            var requestReviewer = _mapper.Map<Reviewer>(requestReviewerDto);

            //Ensure entity has the correct Pokemon and Reviwere attached so that EF can make the relationship
            bool reviewsCreated = _reviewerRepository.CreateReviewer(requestReviewer);
            if (reviewsCreated)
            {
                return true;
            }
            else return false;


        }

        public bool UpdateReviewerToDb(ReviewerDto reviewerUpdate)
        {
            Reviewer oldReviewer = _reviewerRepository.GetReviewer(reviewerUpdate.Id);
            _mapper.Map(reviewerUpdate, oldReviewer);
            bool saved = _reviewerRepository.UpdateReviewer(oldReviewer);
            return saved;
        }


        public bool DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId)) return false;
            Reviewer reviewer = _context.Reviewers.Include(r => r.Reviews)
                .FirstOrDefault(c => c.Id == reviewerId);//Fetch relevant category along with all its dependencies
            if (reviewer.Reviews.Any()) return false; //Do not delete categories with dependent data (delete dependencies first)
            return _reviewerRepository.DeleteReviewer(reviewer);
        }
    }
}
