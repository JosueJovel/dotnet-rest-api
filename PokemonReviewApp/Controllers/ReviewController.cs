using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using PokemonReviewApp.Services;

namespace PokemonReviewApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly ReviewService _reviewService;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper, ReviewService reviewService)
        {
            this._reviewRepository = reviewRepository;
            this._mapper = mapper;
            this._reviewService = reviewService;
        }

        //Get endpoint
        [HttpGet]//Http Get Annotation
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))] //Annotation determing response type
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);//Return IActionResult type with our query
        }


        [HttpGet("{reviewId}")] //endpoint URL extension from our base of "api/[controller]"
        [ProducesResponseType(200, Type = typeof(Review))] //Annotation determing response type
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId)) return NotFound();
            var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(review);//Return IActionResult type with our query
        }

        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsOfPokemon(int pokeId)
        {
            var pokemonReviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfPokemon(pokeId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemonReviews);
        }


        [HttpPost()]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromBody] ReviewDto reviewCreate, [FromQuery] int reviewerId, [FromQuery] int pokeId) //From body is used to grab the request body, equivalent to @RequestBody
        {
            if (reviewCreate == null) return BadRequest(ModelState); //Reject empty request bodies
            bool nameMatch = _reviewService.ReviewNameExists(reviewCreate); //Send ReviewDto to service


            if (nameMatch) //If there was a name match found
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Finally, we have fully verified request is valid. save to repository.
            bool reviewSaved = _reviewService.SaveReviewToDb(reviewCreate, reviewerId, pokeId);
            if (!reviewSaved) //If Review was unable to be saved
            {
                ModelState.AddModelError("", "Something went wrong saving the Review");
                return StatusCode(500, ModelState);
            }


            return Ok("Successfully created");
        }
    }
}
