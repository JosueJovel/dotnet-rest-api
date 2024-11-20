namespace PokemonReviewApp.Models
{
    public class PokemonCategory
    {
        //This is a join table for the many to many relationship between pokemon and categories.
        //EX: A pokemon can be multiple types (water, grass), and a type in our databse (water) will have many pokemon referincing it.
        public int PokemonId { get; set; } //A pokemon's foreign key 
        public int CategoryId { get; set; } //A Category's foreign key
        public Pokemon Pokemon { get; set; } //The actual pokemon entity for a partiuclar row
        public Category Category { get; set; } //The actual Category entity for a particular row


    }
}
