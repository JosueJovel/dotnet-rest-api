using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Services;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly CategoryService _categoryService;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper, CategoryService categoryService) //Constructor inject dependencies
        {
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
            this._categoryService = categoryService;
        }

        //Get endpoint
        [HttpGet]//Http Get Annotation
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))] //Annotation determing response type
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(categories);//Return IActionResult type with our query
        }

        [HttpGet("{categoryId}")] //endpoint URL extension from our base of "api/[controller]"
        [ProducesResponseType(200, Type = typeof(Category))] //Annotation determing response type
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId)) return NotFound();
            var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(categoryId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(category);//Return IActionResult type with our query
        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategoryId(int categoryId)
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonByCategory(categoryId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);//Return IActionResult type with our query
        }

        [HttpPost()]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate) //From body is used to grab the request body, equivalent to @RequestBody
        {
            if(categoryCreate == null) return BadRequest(ModelState);
            //Send categoryDto to service


            bool nameMatch = _categoryService.CategoryNameExists(categoryCreate);

            if (nameMatch) //If there was a name match found
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Finally, we have fully verified request is valid. save to repository.
            bool categorySaved = _categoryService.SaveCategoryToDb(categoryCreate);
            if (!categorySaved) //If category was unable to be saved
            {
                ModelState.AddModelError("", "Something went wrong saving the category");
                return StatusCode(500, ModelState);
            }


            return Ok("Successfully created");
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto categoryUpdate) 
        {
            if (categoryUpdate == null) return BadRequest(ModelState);
            if (categoryUpdate.Id != categoryId) return BadRequest(ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            

            bool saved = _categoryService.UpdateCategoryToDb(categoryUpdate);
            if (!saved)
            {
                ModelState.AddModelError("", "Something went wrong updating the category");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }

    }
}
