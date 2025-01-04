using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services
{
    public class CountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CountryService(ICountryRepository CountryRepository, IMapper mapper, DataContext context)
        {
            this._countryRepository = CountryRepository;
            this._mapper = mapper;
            this._context = context;
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

        public bool UpdateCountryToDb(CountryDto countryDtoUpdate)
        {
            if (_countryRepository.CountryExists(countryDtoUpdate.Id) == false) return false;
            Country countryUpdate = _mapper.Map<Country>(countryDtoUpdate);
            bool saved = _countryRepository.UpdateCountry(countryUpdate);
            return saved;
        }

        internal bool DeleteCountry(int countryId)
        {
            if(_countryRepository.CountryExists(countryId) == false) { return false; } //Cant delete something that doesnt exist
            Country countryToDelete = _context.Countries //Fetch relevant category along with all its dependencies
                .Include(c => c.Owners)
                .FirstOrDefault(c => c.Id == countryId);
            if (countryToDelete.Owners.Any()) { return false; } //Do not delete categories with dependent data (delete dependencies first)
            return _countryRepository.DeleteCountry(countryToDelete);
        }
    }
}
