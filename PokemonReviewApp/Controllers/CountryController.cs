using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")] //Annotations; @ Equivalent from spring boot
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper; //Import mapper, made available for injection as a service in Program.Cs

        //Inject IPokemonRepository and automapper, constructor dependency injection
        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            this._countryRepository = countryRepository;
            this._mapper = mapper;
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
    }

}
