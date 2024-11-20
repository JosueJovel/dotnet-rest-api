namespace PokemonReviewApp.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; } 

        public ICollection<Review> Reviews { get; set; } //Pokemon is the 1 side of a many to many
        public ICollection<PokemonCategory> PokemonCategories { get; set; } //Pokemon is a Many side of a many to many.
        public ICollection<PokemonOwner> PokemonOwners { get; set; }
    }
}
