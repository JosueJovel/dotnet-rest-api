using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository //Our Repository INTERFACE
    {
        ICollection<Pokemon> GetPokemons(); //Our interface method to get pokemon, as a List
    }
}
