using System.ComponentModel.DataAnnotations;

namespace MovieHub.Data.Entities
{
    public class Actor
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string FullName { get; set; } = null!;

        [Range(1850, 2100)]
        public int BirthYear { get; set; }

        public ICollection<MovieActor> MovieActors { get; set; } = new HashSet<MovieActor>();
    }
}
