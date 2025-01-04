using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Services;

namespace PokemonReviewApp.Controllers
{
    //CONTROLLER STRUCTURE
    [Route("api/[controller]")] //Annotations; @ Equivalent from spring boot
    [ApiController]
    public class PokemonController : Controller //This controller class INHERITS the base controller class
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper; //Import mapper, made available for injection as a service in Program.Cs
        private readonly PokemonService _pokemonService;

        //Inject IPokemonRepository and automapper, constructor dependency injection
        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper, PokemonService pokemonService)
        {
            this._pokemonRepository = pokemonRepository;
            this._mapper = mapper;
            this._pokemonService = pokemonService;
        }

        //CONTROLLER ENDPOINT
        [HttpGet]//Http Get Annotation
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))] //Annotation determing response type
        public IActionResult GetPokemons() //Controller endpoint method
        //NOTE ON IACTIONRESULT: It is a return type used to support methods like BadRequest, Ok, NotFound
        //Used to more easily control the kind of reponses we end (Ok, BadRequest, NotFound)
        {
            //Using our repository, not our data context (loose coupling).
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons()); //Use automapper to map pokemon to PokemonDto
            //Typically, we would also use a service method that would actually call the repository method.

            //Checking ModelState, to validate our fetched data
            if (!ModelState.IsValid) //Centralized method of data validation
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemons); //As an endpoint resposne, prvoide Status code 200 OK, and the requested data (pokemon list)
        }

        [HttpGet("{pokeId}")] //endpoint URL extension from our base of "api/[controller]" NOTE: {} are used to mark path variables, just like in spring boot
        [ProducesResponseType(200, Type = typeof(Pokemon))] //Annotation determing response type
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId) //Endpoint to return a single pokemon
        {
            if (!_pokemonRepository.PokemonExists(pokeId)) return NotFound(); //repository validation

            //Using our implemented repository method to fetch a specific pokemon entity by id
            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));//Use automapper to map pokemon to PokemonDto

            //This validates that our incoming requests' values are valid against a DTO's [requirements] (like name being required)
            //Another ex: Our DTO has a pokedex number porpertiy with [Range(1-100)], and the incoming pokedex number is 200
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemon);
        }

        [HttpGet("{pokeId}/rating")] //endpoint URL extension from our base of "api/[controller]"
        [ProducesResponseType(200, Type = typeof(decimal))] //Annotation determing response type
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId)) return NotFound(); //repository validation

            //Using our repository method that calculates an average rating
            decimal rating = _pokemonRepository.GetPokemonRating(pokeId);


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(rating);
        }

        [HttpPost()]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromBody] PokemonDto pokemonCreate, [FromQuery] int ownerId, [FromQuery] int categoryId) //From body is used to grab the request body, equivalent to @RequestBody
        {
            if (pokemonCreate == null) return BadRequest(ModelState); //Reject empty request bodies
            bool nameMatch = _pokemonService.PokemonNameExists(pokemonCreate); //Send PokemonDto to service


            if (nameMatch) //If there was a name match found
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Finally, we have fully verified request is valid. save to repository.
            bool pokemonSaved = _pokemonService.SavePokemonToDb(pokemonCreate, ownerId, categoryId);
            if (!pokemonSaved) //If Pokemon was unable to be saved
            {
                ModelState.AddModelError("", "Something went wrong saving the Pokemon");
                return StatusCode(500, ModelState);
            }


            return Ok("Successfully created");
        }

        [HttpPut("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int pokeId, [FromBody] PokemonDto pokemonUpdate)
        {
            if (pokemonUpdate == null) return BadRequest(ModelState);
            if (pokemonUpdate.Id != pokeId) return BadRequest(ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);


            bool saved = _pokemonService.UpdatePokemonToDb(pokemonUpdate);
            if (!saved)
            {
                ModelState.AddModelError("", "Something went wrong updating the pokemon");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{pokemonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int pokemonId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool saved = _pokemonService.DeletePokemon(pokemonId);
            if (!saved)
            {
                ModelState.AddModelError("", "Something went wrong deleting the pokemon");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
    }
}
