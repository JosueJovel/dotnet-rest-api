using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services
{
    public class CountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryService(ICountryRepository CountryRepository, IMapper mapper)
        {
            this._countryRepository = CountryRepository;
            this._mapper = mapper;
        }

        public bool CountryNameExists(CountryDto CountryDto)
        {
            var namedCountry = _countryRepository.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() == CountryDto.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (namedCountry != null) //If a named match of the Country is found/not null
            {
                return true; //CountryName exists, return true
            }
            else return false; //CountryName does not exist, return false
        }

        public bool SaveCountryToDb(CountryDto requestCountryDto)
        {
            var requestCountry = _mapper.Map<Country>(requestCountryDto);
            bool countriesCreated = _countryRepository.CreateCountry(requestCountry);
            if (countriesCreated)
            {
                return true;
            }
            else return false;
        }

    }
}
