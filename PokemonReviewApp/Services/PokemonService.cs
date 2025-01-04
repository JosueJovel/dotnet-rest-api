using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services
{
    public class PokemonService
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;
        private readonly CountryService _countryService;
        private readonly DataContext _context;

        public PokemonService(IPokemonRepository PokemonRepository, IMapper mapper, ICountryRepository countryRepository, CountryService countryService, DataContext context)
        {
            this._pokemonRepository = PokemonRepository;
            this._mapper = mapper;
            this._countryRepository = countryRepository;
            this._countryService = countryService;
            this._context = context;
        }

        public bool PokemonNameExists(PokemonDto PokemonDto)
        {
            var namedPokemon = _pokemonRepository.GetPokemons()
                .Where(c => c.Name.Trim().ToUpper() == PokemonDto.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (namedPokemon != null) //If a named match of the Pokemon is found/not null
            {
                return true; //PokemonName exists, return true
            }
            else return false; //PokemonName does not exist, return false
        }

        public bool SavePokemonToDb(PokemonDto requestPokemonDto, int ownerId, int categoryId)
        {
            var requestPokemon = _mapper.Map<Pokemon>(requestPokemonDto);
            var pokemonCreated = _pokemonRepository.CreatePokemon(requestPokemon, ownerId, categoryId);
            
            if (pokemonCreated)
            {
                return true;
            }
            else return false;
        }

        public bool UpdatePokemonToDb(PokemonDto pokemonUpdate)
        {
            Pokemon oldPokemon = _pokemonRepository.GetPokemon(pokemonUpdate.Id);
            if (oldPokemon == null) return false;
            _mapper.Map(pokemonUpdate, oldPokemon);
            bool saved = _pokemonRepository.UpdatePokemon(oldPokemon);
            return saved;
        }

        internal bool DeletePokemon(int pokemonId)
        {
            if (_pokemonRepository.PokemonExists(pokemonId)) return false;
            Pokemon pokemon = _context.Pokemon //Bring in pokemon + its dependencies
                .Include(p => p.PokemonOwners)
                .FirstOrDefault(p => p.Id == pokemonId);
            if (pokemon.PokemonOwners.Any() == true || pokemon.PokemonCategories.Any() == true || pokemon.Reviews.Any() == true) return false;
            return _pokemonRepository.DeletePokemon(pokemon);
        }
    }
}
