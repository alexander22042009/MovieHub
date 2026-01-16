using MovieHub.Data.Entities;
using System.Collections.Generic;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int ReleaseYear { get; set; }
    public decimal Rating { get; set; }

    public int GenreId { get; set; }
    public Genre Genre { get; set; } = null!;

    public int AddedByUserId { get; set; }
    public User AddedByUser { get; set; } = null!;

    // ✅ List вместо HashSet
    public List<MovieActor> MovieActors { get; set; } = new();
}
