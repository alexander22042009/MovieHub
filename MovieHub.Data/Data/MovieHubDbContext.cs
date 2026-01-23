using Microsoft.EntityFrameworkCore;
using MovieHub.Data.Entities;
using MovieHub.Data.Configurations;


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
        public DbSet<Actor> Actors { get; set; } = null!;
        public DbSet<MovieActor> MovieActors { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new GenreConfiguration());
            modelBuilder.ApplyConfiguration(new MovieConfiguration());
            modelBuilder.ApplyConfiguration(new ActorConfiguration());
            modelBuilder.ApplyConfiguration(new MovieActorConfiguration());
            base.OnModelCreating(modelBuilder);
        }

    }
}
