using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services
{
    public class OwnerService
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;
        private readonly CountryService _countryService;
        private readonly DataContext _context;

        public OwnerService(IOwnerRepository OwnerRepository, IMapper mapper, ICountryRepository countryRepository, CountryService countryService, DataContext context)
        {
            this._ownerRepository = OwnerRepository;
            this._mapper = mapper;
            this._countryRepository = countryRepository;
            this._countryService = countryService;
            this._context = context;
        }

        public bool OwnerNameExists(OwnerDto OwnerDto)
        {
            var namedOwner = _ownerRepository.GetOwners()
                .Where(c => c.Name.Trim().ToUpper() == OwnerDto.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (namedOwner != null) //If a named match of the Owner is found/not null
            {
                return true; //OwnerName exists, return true
            }
            else return false; //OwnerName does not exist, return false
        }

        public bool SaveOwnerToDb(OwnerDto requestOwnerDto, int countryId)
        {
            var requestOwner = _mapper.Map<Owner>(requestOwnerDto);

            //Check if country exists. If it doesnt, create a new one (only name needed)
            bool countryNameExists = _countryRepository.CountryExists(countryId);

            if (!countryNameExists) //If given country does not exist
            {
                return false;//indicate an error by sending false
            }

            //Now that we know owner's country exists in the DB, grab it back and insert it into this entity
            requestOwner.Country = _countryRepository.GetCountry(countryId);
            
            //Now that owner is fully completed, create it in the DB
            bool ownersCreated = _ownerRepository.CreateOwner(requestOwner);
            if (ownersCreated)
            {
                return true;
            }
            else return false;
        }

        public bool UpdateOwnerToDb(OwnerDto ownerUpdate)
        {
            var dbOwner = _ownerRepository.GetOwner(ownerUpdate.Id); //Get the ownwer we are updating from DB
            if (dbOwner != null) //If owner does indeed exist
            { //update owner values
                dbOwner.Name = ownerUpdate.Name;
                dbOwner.Gym = ownerUpdate.Gym;
            }
            var saved = _ownerRepository.UpdateOwner(dbOwner); //save this owner into DB
            return saved;
        }

        internal bool DeleteOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId)) return false;
            Owner owner = _context.Owners
                .Include(o => o.PokemonOwners)
                .FirstOrDefault(c => c.Id == ownerId);//Fetch relevant category along with all its dependencies
            if (owner.PokemonOwners.Any()) return false; //Do not delete categories with dependent data (delete dependencies first)
            return _ownerRepository.DeleteOwner(owner);
        }
    }
}
