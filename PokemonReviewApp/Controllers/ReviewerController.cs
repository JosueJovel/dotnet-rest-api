using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using PokemonReviewApp.Services;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")] //Annotations; @ Equivalent from spring boot
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;
        private readonly ReviewerService _reviewerService;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper, ReviewerService reviewerService)
        {
            this._reviewerRepository = reviewerRepository;
            this._mapper = mapper;
            this._reviewerService = reviewerService;
        }

        //CONTROLLER ENDPOINT
        [HttpGet]//Http Get Annotation
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))] //Annotation determing response type
        public IActionResult GetReviewers() //Controller endpoint method
        {
            var reviewers = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int reviewerId)
        {
            if(!_reviewerRepository.ReviewerExists(reviewerId)) return NotFound();
            var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(reviewerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviewer);
        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId)) return NotFound();
            var reviewerReviews = _mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewsByReviewer(reviewerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviewerReviews);
        }
        [HttpPost()]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerCreate) //From body is used to grab the request body, equivalent to @RequestBody
        {
            if (reviewerCreate == null) return BadRequest(ModelState); //Reject empty request bodies
            bool nameMatch = _reviewerService.ReviewerNameExists(reviewerCreate); //Send ReviewerDto to service


            if (nameMatch) //If there was a name match found
            {
                ModelState.AddModelError("", "Reviewer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Finally, we have fully verified request is valid. save to repository.
            bool reviewerSaved = _reviewerService.SaveReviewerToDb(reviewerCreate);
            if (!reviewerSaved) //If Reviewer was unable to be saved
            {
                ModelState.AddModelError("", "Something went wrong saving the Reviewer");
                return StatusCode(500, ModelState);
            }


            return Ok("Successfully created");
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDto reviewerUpdate)
        {
            if (reviewerUpdate == null) return BadRequest(ModelState);
            if (reviewerUpdate.Id != reviewerId) return BadRequest(ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);


            bool saved = _reviewerService.UpdateReviewerToDb(reviewerUpdate);
            if (!saved)
            {
                ModelState.AddModelError("", "Something went wrong updating the reviewer");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
