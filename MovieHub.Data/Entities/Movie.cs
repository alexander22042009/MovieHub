using MovieHub.Data.Entities;

public class Movie
{
    public int Id { get; set; }

    public string Title { get; set; } = "";

    public int ReleaseYear { get; set; }

    public decimal Rating { get; set; }  // ⬅ виж проблем 2

    public int GenreId { get; set; }
    public Genre Genre { get; set; } = null!;

    public int AddedByUserId { get; set; }
    public User AddedByUser { get; set; } = null!;

    public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
}
