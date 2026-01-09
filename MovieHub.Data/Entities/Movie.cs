using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Data.Entities
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string Title { get; set; } = null!;

        [Range(1888, 2100)]
        public int ReleaseYear { get; set; }

        [Range(0, 10)]
        [Column(TypeName = "decimal(3,1)")]
        public decimal Rating { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; } = null!;

        public int AddedByUserId { get; set; }
        public User AddedByUser { get; set; } = null!;

        public ICollection<MovieActor> MovieActors { get; set; } = new HashSet<MovieActor>();
    }
}
