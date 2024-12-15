using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository //This implementation IMPLEMENTS our repository interface
    {
        private readonly DataContext _context;

        public PokemonRepository(DataContext context) //DataContext constructor injection
        {
            this._context = context;
        }

        public ICollection<Pokemon> GetPokemons() //Implementation of our pokemon repository interfacce method
        {
            return _context.Pokemon.OrderBy(p => p.Id).ToList(); //Use our DataContext;s methods to grab our desired data from the DB
            //NOTICE: The repostiory EMPLOYS the data context to interact with the database. But the context does the actual operation.
            //This shows how repostories are used for structrual/organizational uses.
        }
    }
}
