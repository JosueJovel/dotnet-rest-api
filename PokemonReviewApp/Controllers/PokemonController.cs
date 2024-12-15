using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    //CONTROLLER STRUCTURE
    [Route("api/[controller]")] //Annotations; @ Equivalent from spring boot
    [ApiController]
    public class PokemonController : Controller //This controller class INHERITS the base controller class
    {
        private readonly IPokemonRepository _pokemonRepository;

        public PokemonController(IPokemonRepository pokemonRepository)//Inject IPokemonRepository
        {
            this._pokemonRepository = pokemonRepository;
        }

        //CONTROLLER ENDPOINT
        [HttpGet]//Http Get Annotation
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))] //Annotation determing response type
        public IActionResult GetPokemons() //Controller endpoint method
        {
            var pokemons = _pokemonRepository.GetPokemons(); //Using our repository, not our data context (loose coupling)
            //Typically, we would also use a service method that would actually call the repository method.

            //Checking ModelState, to validate our fetched data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemons); //As an endpoint resposne, prvoide Status code 200 OK, and the requested data (pokemon list)
        }

    }
}
