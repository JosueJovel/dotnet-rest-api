using AutoMapper;
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

        public OwnerService(IOwnerRepository OwnerRepository, IMapper mapper, ICountryRepository countryRepository, CountryService countryService)
        {
            this._ownerRepository = OwnerRepository;
            this._mapper = mapper;
            this._countryRepository = countryRepository;
            this._countryService = countryService;
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

    }
}
