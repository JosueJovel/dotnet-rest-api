using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories(); //Fetch list of all categories from table
        Category GetCategory(int id);
        ICollection<Pokemon> GetPokemonByCategory(int categoryId);//Query to fetch all pokemons with a specified category id
        bool CategoryExists(int id);

        bool CreateCategory(Category category); //CREATEE repository method signature
        bool Save();
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
    }
}
