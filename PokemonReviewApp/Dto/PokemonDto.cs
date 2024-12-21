namespace PokemonReviewApp.Dto
{
    public class PokemonDto //This DATA TRANSFER OBJECT is a simplified version of the real entity our controller will use
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
