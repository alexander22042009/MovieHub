using System.ComponentModel.DataAnnotations;

namespace MovieHub.Data.Entities
{
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        public ICollection<Movie> Movies { get; set; } = new HashSet<Movie>();
    }
}
