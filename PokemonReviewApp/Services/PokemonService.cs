using AutoMapper;
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

        public PokemonService(IPokemonRepository PokemonRepository, IMapper mapper, ICountryRepository countryRepository, CountryService countryService)
        {
            this._pokemonRepository = PokemonRepository;
            this._mapper = mapper;
            this._countryRepository = countryRepository;
            this._countryService = countryService;
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

    }
}
