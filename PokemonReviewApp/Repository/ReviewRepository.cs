using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _dataContext;

        public ReviewRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool CreateReview(Review review)
        {
            _dataContext.Add(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _dataContext.Remove(review);
            return Save();
        }

        public Review GetReview(int reviewId)
        {
            return _dataContext.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _dataContext.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsOfPokemon(int pokeId)
        {
            return _dataContext.Reviews.Where(r => r.Pokemon.Id == pokeId).ToList();
        }

        public bool ReviewExists(int reviewId)
        {
            return _dataContext.Reviews.Any(r => r.Id == reviewId);
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges(); //Formally write/send stored changes/db transaction to the db.
            return saved > 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
            _dataContext.Update(review);
            return Save();
        }
    }
}
