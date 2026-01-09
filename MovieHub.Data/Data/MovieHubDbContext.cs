using Microsoft.EntityFrameworkCore;
using MovieHub.Data.Entities;

namespace MovieHub.Data.Data
{
    public class MovieHubDbContext : DbContext
    {
        public MovieHubDbContext() { }

        public MovieHubDbContext(DbContextOptions<MovieHubDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Actor> Actors => Set<Actor>();
        public DbSet<MovieActor> MoviesActors => Set<MovieActor>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent configurations (ще ги добавим в следващите стъпки)
            base.OnModelCreating(modelBuilder);
        }
    }
}
