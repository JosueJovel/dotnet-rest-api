using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper) 
        {
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
        }

        public bool CategoryNameExists(CategoryDto categoryDto)
        {
            var namedCategory = _categoryRepository.GetCategories()
                .Where(c => c.Name.Trim().ToUpper() == categoryDto.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (namedCategory != null) //If a named match of the category is found/not null
            {
                return true; //CateogryName exists, return true
            }
            else return false; //CateogryName does not exist, return false
        }

        public bool SaveCategoryToDb(CategoryDto requestCategoryDto)
        {
            var requestCategory = _mapper.Map<Category>(requestCategoryDto);
            bool categoriesCreated = _categoryRepository.CreateCategory(requestCategory);
            if (categoriesCreated)
            {
                return true;
            } else return false;
        }

        public bool UpdateCategoryToDb(CategoryDto categoryUpdate)
        {
            //check if entry exists with this id
            if (_categoryRepository.CategoryExists(categoryUpdate.Id) == false) return false;

            //If it does, we are good to go
            Category updatedCategory = _mapper.Map<Category>(categoryUpdate);
            bool saved = _categoryRepository.UpdateCategory(updatedCategory);
            return saved; 
            
        }

    }
}
