using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            this._context = context;
        }
        public bool CategoryExists(int id)
        {
            //These LINQ methods expect Lambda filter functions, using the structure (x => x == condition)
            return _context.Categories.Any(c => c.Id == id); //Any == "exists", just a bool to check if exists.
        }

        public bool CreateCategory(Category category)
        {
            //DB transactions
            _context.Add(category);

            return Save(); //Formally submit your DB Transaction
        }

        public ICollection<Category> GetCategories()
        {
            //"Categories" itself is the whole table, now we simple turn that table into a list.
            return _context.Categories.ToList(); 
        }

        public Category GetCategory(int id)
        {
            //Since we want to grab result with a condition, use LINQ's Where()
            return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
            return _context.PokemonCategories.Where(p => p.CategoryId == categoryId).Select(c => c.Pokemon).ToList();
            //Detailed breakdown: Within our PokemonCategories join table, we grab filtered results based on the condition of
            //only grabbing results with out category ids. We also use Select() to grab specifically only the Pokemon category
            //of our PokemonCategory entity, since it has both a Pokemon and Category entity within it.
            //NOTE: LINQ methods like the ones above rely on you using them like they are manipulating the entities directly.
            //Instead, ENtity framework will later convert the above query into an actual sql statment to deliver the desired result
            //In that way, you can think of these LINQ methods as a form of abstraction to get you to not worry about SQL cosntructiosn
            //or complex queries like joins, and instead simply and easily interact with entities directly.
            //EX: When we say "Select(c => c.Pokemon)",
            //EF reads "Select Pokemon.Id, Pokemon.Name, Pokemon.type.... FROM PokemonCategories INNER JOIN Pokemon)
            //Without Select, we would get EVERYTHING, including nulls from other entities not being in the DB
        }

        public bool Save()
        {
            var saved = _context.SaveChanges(); //Formally write/send stored changes/db transaction to the db.
            return saved > 0 ? true : false;
        }
    }
}
