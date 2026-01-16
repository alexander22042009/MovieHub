namespace MovieHub.App.DTOs.Movies
{
    public class EditMovieDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = "";
        public int Year { get; set; }
        public int GenreId { get; set; }
        public decimal Rating { get; set; }
    }
}

