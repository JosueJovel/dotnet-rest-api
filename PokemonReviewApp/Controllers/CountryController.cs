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
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper; //Import mapper, made available for injection as a service in Program.Cs
        private readonly CountryService _countryService;

        //Inject IPokemonRepository and automapper, constructor dependency injection
        public CountryController(ICountryRepository countryRepository, IMapper mapper, CountryService countryService)
        {
            this._countryRepository = countryRepository;
            this._mapper = mapper;
            this._countryService = countryService;
        }

        //CONTROLLER ENDPOINT
        [HttpGet]//Http Get Annotation
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))] //Annotation determing response type
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

            //Checking ModelState, to validate our fetched data
            if (!ModelState.IsValid) //Centralized method of data validation
            {
                return BadRequest(ModelState);
            }

            return Ok(countries);
        }

        [HttpGet("{countryId}")] //endpoint URL extension from our base of "api/[controller]" NOTE: {} are used to mark path variables, just like in spring boot
        [ProducesResponseType(200, Type = typeof(Country))] //Annotation determing response type
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int countryId) //Endpoint to return a single country
        {
            if (!_countryRepository.CountryExists(countryId)) return NotFound(); //repository validation

            //Using our implemented repository method to fetch a specific country entity(and mapped to dto) by id
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));//Use automapper to map country to countryDto

            //This validates that our incoming requests' values are valid against a DTO's [requirements] (like name being required)
            //Another ex: Our DTO has a pokedex number porpertiy with [Range(1-100)], and the incoming pokedex number is 200
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(country);
        }

        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(Country))]
        public IActionResult GetCountryOfOwner(int ownerId) 
        {
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(country);
        }

        [HttpPost()]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDto countryCreate) //From body is used to grab the request body, equivalent to @RequestBody
        {
            if (countryCreate == null) return BadRequest(ModelState); //Reject empty request bodies
            bool nameMatch = _countryService.CountryNameExists(countryCreate); //Send CountryDto to service


            if (nameMatch) //If there was a name match found
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Finally, we have fully verified request is valid. save to repository.
            bool countrySaved = _countryService.SaveCountryToDb(countryCreate);
            if (!countrySaved) //If Country was unable to be saved
            {
                ModelState.AddModelError("", "Something went wrong saving the Country");
                return StatusCode(500, ModelState);
            }


            return Ok("Successfully created");
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto countryUpdate)
        {
            if (countryUpdate == null) return BadRequest(ModelState);
            if (countryUpdate.Id != countryId) return BadRequest(ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);


            bool saved = _countryService.UpdateCountryToDb(countryUpdate);
            if (!saved)
            {
                ModelState.AddModelError("", "Something went wrong updating the country");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool saved = _countryService.DeleteCountry(countryId);
            if (!saved)
            {
                ModelState.AddModelError("", "Something went wrong deleting the country");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
    }

}
