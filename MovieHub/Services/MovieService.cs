using Microsoft.EntityFrameworkCore;
using MovieHub.App.DTOs.Movies;
using MovieHub.Data.Data;
using MovieHub.Data.Entities;

namespace MovieHub.App.Services
{
    public class MovieService
    {
        private readonly MovieHubDbContext _db;

        public MovieService(MovieHubDbContext db)
        {
            _db = db;
        }

        public async Task<List<MovieListItemDto>> GetAllMoviesAsync()
        {
            return await _db.Movies
                .AsNoTracking()
                .Include(m => m.Genre)
                .Include(m => m.AddedByUser)
                .OrderBy(m => m.Title)
                .Select(m => new MovieListItemDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    ReleaseYear = m.ReleaseYear,
                    Genre = m.Genre.Name,
                    Rating = m.Rating,
                    AddedBy = m.AddedByUser.Username
                })
                .ToListAsync();
        }

        public async Task<List<MovieListItemDto>> SearchMoviesAsync(string text)
        {
            text = text.Trim();

            return await _db.Movies
                .AsNoTracking()
                .Include(m => m.Genre)
                .Include(m => m.AddedByUser)
                .Where(m => m.Title.Contains(text))
                .OrderBy(m => m.Title)
                .Select(m => new MovieListItemDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    ReleaseYear = m.ReleaseYear,
                    Genre = m.Genre.Name,
                    Rating = m.Rating,
                    AddedBy = m.AddedByUser.Username
                })
                .ToListAsync();
        }

        public async Task<(bool ok, string message)> AddMovieAsync(AddMovieDto dto, int currentUserId)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                return (false, "Title is required.");

            if (dto.Year < 1888 || dto.Year > DateTime.Now.Year + 1)
                return (false, "Invalid year.");

            var genreExists = await _db.Genres.AnyAsync(g => g.Id == dto.GenreId);
            if (!genreExists)
                return (false, "Invalid genre.");

            if (dto.Rating < 0 || dto.Rating > 10)
                return (false, "Rating must be between 0 and 10.");

            var movie = new Movie
            {
                Title = dto.Title.Trim(),
                ReleaseYear = dto.Year,
                GenreId = dto.GenreId,
                Rating = dto.Rating,
                AddedByUserId = currentUserId
            };

            _db.Movies.Add(movie);
            await _db.SaveChangesAsync();

            return (true, $"Movie '{movie.Title}' added.");
        }

        public async Task<(bool ok, string message)> EditMovieAsync(EditMovieDto dto, int currentUserId, bool isAdmin)
        {
            var movie = await _db.Movies.FirstOrDefaultAsync(m => m.Id == dto.MovieId);
            if (movie == null)
                return (false, "Movie not found.");

            if (!isAdmin && movie.AddedByUserId != currentUserId)
                return (false, "You can edit only your own movies.");

            if (string.IsNullOrWhiteSpace(dto.Title))
                return (false, "Title is required.");

            if (dto.Year < 1888 || dto.Year > DateTime.Now.Year + 1)
                return (false, "Invalid year.");

            var genreExists = await _db.Genres.AnyAsync(g => g.Id == dto.GenreId);
            if (!genreExists)
                return (false, "Invalid genre.");

            if (dto.Rating < 0 || dto.Rating > 10)
                return (false, "Rating must be between 0 and 10.");

            movie.Title = dto.Title.Trim();
            movie.ReleaseYear = dto.Year;
            movie.GenreId = dto.GenreId;
            movie.Rating = dto.Rating;

            await _db.SaveChangesAsync();
            return (true, $"Movie '{movie.Title}' updated.");
        }

        public async Task<(bool ok, string message)> DeleteMovieAsync(int movieId, int currentUserId, bool isAdmin)
        {
            var movie = await _db.Movies.FirstOrDefaultAsync(m => m.Id == movieId);
            if (movie == null)
                return (false, "Movie not found.");

            if (!isAdmin && movie.AddedByUserId != currentUserId)
                return (false, "You can delete only your own movies.");

            _db.Movies.Remove(movie);
            await _db.SaveChangesAsync();

            return (true, "Movie deleted.");
        }

        public async Task<List<(int id, string name)>> GetGenresAsync()
        {
            return await _db.Genres
                .AsNoTracking()
                .OrderBy(g => g.Name)
                .Select(g => new ValueTuple<int, string>(g.Id, g.Name))
                .ToListAsync();
        }
    }
}
