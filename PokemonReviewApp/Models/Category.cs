namespace PokemonReviewApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<PokemonCategory> PokemonCategories { get; set; } //A category may be referenced many times in the Pokemon-Category join table. Many to many.
    }
}
