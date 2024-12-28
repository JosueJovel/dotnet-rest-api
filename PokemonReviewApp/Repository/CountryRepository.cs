using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public CountryRepository(DataContext dataContext, IMapper mapper)
        {
            this._dataContext = dataContext;
            this._mapper = mapper;
        }
        public bool CountryExists(int id)
        {
            //Grab all of our countries from our data context DBSet. Then apply the "Any" LINQ method to that set.
            return _dataContext.Countries.Any(country => country.Id == id);
        }

        public ICollection<Country> GetCountries()
        {
            return _dataContext.Countries.ToList();
            //Grab all country entities from the DB Set. In sql, this will translate to grab all entries
            //of the Countries table.
        }

        public Country GetCountry(int id)
        {
            return _dataContext.Countries.Where(c => c.Id == id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return _dataContext.Owners.Where(o => o.Id == ownerId).Select(o => o.Country).FirstOrDefault();
            //BREAKDOWN: Within the owners entity DBSET, grab all owners (1 owner) where their id matches the passed owner id
            //Then, select specifically the country entity of those owners. SQL wise, it will grab any matching Country entries
            //From the Country table, which as a one to many relationship with the Owners table.
        }
        public ICollection<Owner> GetOwnersFromCountry(int countryId)
        {
            return _dataContext.Countries.Where(c => c.Id == countryId).SelectMany(c => c.Owners).ToList();
            //SELECT MANY: Instead of grabbing sevearl Lists of Owners, combine all Owners entries of each list into 
            //one, greater List of Owners

            //return _dataContext.Owners.Where(c => c.Country.Id == countryId).ToList();
        }
    }
}
