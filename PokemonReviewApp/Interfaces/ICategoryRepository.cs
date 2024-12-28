﻿using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories(); //Fetch list of all categories from table
        Category GetCategory(int id);
        ICollection<Pokemon> GetPokemonByCategory(int categoryId);//Query to fetch all pokemons with a specified category id
        bool CategoryExists(int id);

    }
}
