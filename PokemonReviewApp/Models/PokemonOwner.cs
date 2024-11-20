namespace PokemonReviewApp.Models
{
    public class PokemonOwner
    {
        //This is a join table to represent the many to many relationships between pokemon and owners.
        //A trainer will own multiple pokemon, and pokemon may have multiple trainers
        public int PokemonId { get; set; }
        public int OwnerId { get; set; }
        public Pokemon Pokemon { get; set; }
        public Owner Owner { get; set; }

    }
}
