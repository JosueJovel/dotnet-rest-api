using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services
{
    public class ReviewerService
    {
        private readonly IMapper _mapper;
        private IReviewerRepository _reviewerRepository;

        public ReviewerService(IMapper mapper, IReviewerRepository reviewerRepository)
        {
            this._mapper = mapper;
            this._reviewerRepository = reviewerRepository;
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

    }
}
