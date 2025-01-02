using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository //Our Repository INTERFACE
    {
        ICollection<Pokemon> GetPokemons(); //Our interface method to get pokemon, as a List
        Pokemon GetPokemon(int id); //Interface method for fetching a Pokemon entity from DB, by ID
        Pokemon GetPokemon(string name);//Interface method for fetching a Pokemon entity from DB, by name
        decimal GetPokemonRating(int pokeId); //Interface method for fetching a Pokemon rating from DB, by ID
        bool PokemonExists(int pokeId); //Interface method for checking if a pokemon exists in the DB, given a pokemon ID
        bool CreatePokemon(Pokemon pokemon, int ownerId, int categoryId); //CREATE repository method signature
        bool Save();
        bool UpdatePokemon(Pokemon pokemon);
    }
}
