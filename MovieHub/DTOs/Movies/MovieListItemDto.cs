namespace MovieHub.App.DTOs.Movies
{
    public class MovieListItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public int ReleaseYear { get; set; }
        public string Genre { get; set; } = "";
        public decimal Rating { get; set; }
        public string AddedBy { get; set; } = "";
    }
}
