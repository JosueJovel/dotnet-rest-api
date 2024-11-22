using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {

        }

        //Regular Tables
        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pokemon> Pokemon { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }


        public DbSet<PokemonCategory> PokemonCategories { get; set; } //Join Table
        public DbSet<PokemonOwner> PokemonOwners { get; set; } //Join Table


        //This establishes the many-many relationships g
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokemonCategory>().HasKey(pc => new { pc.PokemonId, pc.CategoryId });
            modelBuilder.Entity<PokemonCategory>().HasOne(p => p.Pokemon).WithMany(pc => pc.PokemonCategories).HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonCategory>().HasOne(p => p.Category).WithMany(pc => pc.PokemonCategories).HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<PokemonOwner>().HasKey(po => new { po.PokemonId, po.OwnerId });
            modelBuilder.Entity<PokemonOwner>().HasOne(p => p.Pokemon).WithMany(pc => pc.PokemonOwners).HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonOwner>().HasOne(p => p.Owner).WithMany(pc => pc.PokemonOwners).HasForeignKey(c => c.OwnerId);

        }

    }
}
