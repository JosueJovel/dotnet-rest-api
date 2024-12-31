using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services
{
    public class ReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private IPokemonRepository _pokemonRepository;
        private IReviewerRepository _reviewerRepository;

        public ReviewService(IReviewRepository ReviewRepository, IMapper mapper, IPokemonRepository pokemonRepository, IReviewerRepository reviewerRepository)
        {
            this._reviewRepository = ReviewRepository;
            this._mapper = mapper;
            this._pokemonRepository = pokemonRepository;
            this._reviewerRepository = reviewerRepository;
        }

        public bool ReviewNameExists(ReviewDto ReviewDto)
        {
            var namedReview = _reviewRepository.GetReviews()
                .Where(c => c.Title.Trim().ToUpper() == ReviewDto.Title.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (namedReview != null) //If a named match of the Review is found/not null
            {
                return true; //ReviewName exists, return true
            }
            else return false; //ReviewName does not exist, return false
        }

        public bool SaveReviewToDb(ReviewDto requestReviewDto, int reviewerId, int pokeId)
        {
            var requestReview = _mapper.Map<Review>(requestReviewDto);

            //Ensure entity has the correct Pokemon and Reviwere attached so that EF can make the relationship
            requestReview.Pokemon = _pokemonRepository.GetPokemon(pokeId);
            requestReview.Reviewer = _reviewerRepository.GetReviewer(reviewerId);

            bool reviewsCreated = _reviewRepository.CreateReview(requestReview);
            if (reviewsCreated)
            {
                return true;
            }
            else return false;


        }

    }
}
