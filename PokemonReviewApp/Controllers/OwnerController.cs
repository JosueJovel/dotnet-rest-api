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
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        private readonly OwnerService _ownerService;

        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper, OwnerService ownerService)
        {
            this._ownerRepository = ownerRepository;
            this._mapper = mapper;
            this._ownerService = ownerService;
        }

        //CONTROLLER ENDPOINT
        [HttpGet]//Http Get Annotation
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))] //Annotation determing response type
        public IActionResult GetOwners() //Controller endpoint method
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owners);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId)) return NotFound(); //repository validation

            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));

            //This validates that our incoming requests' values are valid against a DTO's [requirements] (like name being required)
            //Another ex: Our DTO has a pokedex number porpertiy with [Range(1-100)], and the incoming pokedex number is 200
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owner);
        }

        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId)) return NotFound(); //repository validation
            
            var ownerPokemons = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonByOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(ownerPokemons);
        }

        [HttpPost()]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromBody] OwnerDto ownerCreate, [FromQuery] int countryId) //From body is used to grab the request body, equivalent to @RequestBody
        {
            if (ownerCreate == null) return BadRequest(ModelState); //Reject empty request bodies
            bool nameMatch = _ownerService.OwnerNameExists(ownerCreate); //Send OwnerDto to service


            if (nameMatch) //If there was a name match found
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Finally, we have fully verified request is valid. save to repository.
            bool ownerSaved = _ownerService.SaveOwnerToDb(ownerCreate, countryId);
            if (!ownerSaved) //If Owner was unable to be saved
            {
                ModelState.AddModelError("", "Something went wrong saving the Owner");
                return StatusCode(500, ModelState);
            }


            return Ok("Successfully created");
        }
        [HttpPut("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto ownerUpdate)
        {
            if (ownerUpdate == null) return BadRequest(ModelState);
            if (ownerUpdate.Id != ownerId) return BadRequest(ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);


            bool saved = _ownerService.UpdateOwnerToDb(ownerUpdate);
            if (!saved)
            {
                ModelState.AddModelError("", "Something went wrong updating the owner");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOwner(int ownerId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool saved = _ownerService.DeleteOwner(ownerId);
            if (!saved)
            {
                ModelState.AddModelError("", "Something went wrong deleting the owner");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }

    }
}
