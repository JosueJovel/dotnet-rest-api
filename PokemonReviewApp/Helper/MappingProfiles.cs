﻿using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Helper
{
    public class MappingProfiles : Profile //Our AUTOMAPPER class to map entities to DTOS (MapStruct equivalent)
    {
        public MappingProfiles() 
        {
            CreateMap<Pokemon, PokemonDto>(); //Create a mapping between Pokemon and PokemonDto. Fields are name matched
            CreateMap<Category, CategoryDto>();
            CreateMap<Country, CountryDto>();
            CreateMap<Owner, OwnerDto>();
            CreateMap<Review, ReviewDto>();
            CreateMap<Reviewer, ReviewerDto>();
            CreateMap<CategoryDto, Category>(); //Mapping from Dto to Entity, usually for mapping request bodies
            CreateMap<CountryDto, Country>();
            CreateMap<OwnerDto, Owner>();
            CreateMap<PokemonDto, Pokemon>();
            CreateMap<ReviewDto, Review>();
            CreateMap<ReviewerDto, Reviewer>();
        }
    }
}
