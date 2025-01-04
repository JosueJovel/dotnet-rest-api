using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Linq;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository //This implementation IMPLEMENTS our repository interface
    {
        private readonly DataContext _context; //This is the actual context that interacts with the database

        public PokemonRepository(DataContext context) //DataContext constructor injection
        {
            this._context = context;
        }

        public bool CreatePokemon(Pokemon pokemon, int ownerId, int categoryId)
        {
            Pokemon newPokemon = pokemon;
            Owner newOwner = _context.Owners.Where(po => po.Id == ownerId).FirstOrDefault();
            Category category = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

            //Because its a MANY TO MANY realtionship, we need to construct the new entry on the many to many table first, THENN reference it wihtin our new pokemon.
            PokemonOwner newPokemonOwner = new() //our new pokemon owner, this creates the associatoin between PokemonOwner and Pokemon in the databsae.
            {
                Owner = newOwner,
                Pokemon = newPokemon
            };

            PokemonCategory newPokemonCategory = new() //our new pokemon owner, this creates the associatoin between PokemonOwner and Pokemon in the databsae.
            {
                Category = category,
                Pokemon = newPokemon
            };

            _context.Add(newPokemonOwner); //Save our new pokemon owner relationship
            _context.Add(newPokemonCategory);//Save new pokemon category relationship

            _context.Add(pokemon); //Now that all relationships are correctly build, save pokemon
            return Save();
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            throw new NotImplementedException();
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

        public bool Save()
        {
            var saved = _context.SaveChanges(); //Formally write/send stored changes/db transaction to the db.
            return saved > 0 ? true : false;
        }

        public bool UpdatePokemon(Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }
    }
}
