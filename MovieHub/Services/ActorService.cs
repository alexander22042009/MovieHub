using Microsoft.EntityFrameworkCore;
using MovieHub.Data.Data;
using MovieHub.Data.Entities;

namespace MovieHub.App.Services
{
    public class ActorService
    {
        private readonly MovieHubDbContext _db;

        public ActorService(MovieHubDbContext db)
        {
            _db = db;
        }

        public async Task<List<(int id, string name)>> GetAllActorsAsync()
        {
            return await _db.Actors
                .OrderBy(a => a.FullName)
                .Select(a => new ValueTuple<int, string>(a.Id, a.FullName))
                .ToListAsync();
        }

        public async Task<(bool ok, string message)> AddActorAsync(string fullName)
        {
            fullName = (fullName ?? "").Trim();
            if (string.IsNullOrWhiteSpace(fullName))
                return (false, "Actor name is required.");

            var exists = await _db.Actors.AnyAsync(a => a.FullName == fullName);
            if (exists)
                return (false, "Actor already exists.");

            _db.Actors.Add(new Actor { FullName = fullName });
            await _db.SaveChangesAsync();

            return (true, "Actor added.");
        }

        public async Task<(bool ok, string message)> AddActorToMovieAsync(int movieId, int actorId)
        {
            if (movieId <= 0 || actorId <= 0)
                return (false, "Invalid ids.");

            var movieExists = await _db.Movies.AnyAsync(m => m.Id == movieId);
            if (!movieExists) return (false, "Movie not found.");

            var actorExists = await _db.Actors.AnyAsync(a => a.Id == actorId);
            if (!actorExists) return (false, "Actor not found.");

            var linkExists = await _db.MovieActors.AnyAsync(ma => ma.MovieId == movieId && ma.ActorId == actorId);
            if (linkExists) return (false, "This actor is already linked to the movie.");

            _db.MovieActors.Add(new MovieActor { MovieId = movieId, ActorId = actorId });
            await _db.SaveChangesAsync();

            return (true, "Actor linked to movie.");
        }

        public async Task<List<string>> GetActorsForMovieAsync(int movieId)
        {
            return await _db.MovieActors
                .Where(ma => ma.MovieId == movieId)
                .Select(ma => ma.Actor.FullName)
                .OrderBy(n => n)
                .ToListAsync();
        }
    }
}
