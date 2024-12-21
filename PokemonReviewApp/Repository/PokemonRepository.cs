using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository //This implementation IMPLEMENTS our repository interface
    {
        private readonly DataContext _context; //This is the actual context that interacts with the database

        public PokemonRepository(DataContext context) //DataContext constructor injection
        {
            this._context = context;
        }

        public Pokemon GetPokemon(int id) //Repository method for fetching a specific pokemon by ID
        {
            return _context.Pokemon.Where(p => p.Id == id).FirstOrDefault(); //Executing Where Query, return first pkemon result or default value
            //p stands for whatever pokemon entity the filter function is evaluating
        }

        public Pokemon GetPokemon(string name) //Repository method for fetching a specific pokemon by name
        {
            return _context.Pokemon.Where(p => p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeId) //Method 
        {
            var reviews = _context.Reviews.Where(p => p.Id == pokeId); //fetch all of a pokemon's reviews(list)
            //Where and Select LINQ extension methods do not execute the query immedeatly.

            //NOTE: The below logic would be better put in a service, as it is business logic.
            if (reviews.Count() <= 0) //If a pokemon's review count is 0 or less
            {
                return 0;
            }

            return ((decimal)reviews.Sum(r => r.Rating) / reviews.Count()); //Return the AVERAGE of all of a pokemon's ratings
            //NOTE: Count() and Sum() are LINQ Extension methods, meaning the modify the query before sending it to the database.
            //Count/Sum trigger query execution.
            //Extension methods are a C# feature that allows you to append methods to an existing type without modifying the type itself.
            //They basically "extend" interfaces without actually modifying them.
            //IQueryable, such as reviews, is a "built up" query expression that is ready to be sent to the database when it is needed.
        }

        public ICollection<Pokemon> GetPokemons() //Implementation of our pokemon repository interfacce method
        {
            return _context.Pokemon.OrderBy(p => p.Id).ToList(); //Use our DataContext;s methods to grab our desired data from the DB
            //NOTICE: The repostiory EMPLOYS the data context to interact with the database. But the context does the actual operation.
            //This shows how repostories are used for structrual/organizational uses.
        }

        public bool PokemonExists(int pokeId)
        {
            return _context.Pokemon.Any(p => p.Id == pokeId);//Using the Any LINQ method to verify if a certain pokemon exists or not
        }
    }
}
