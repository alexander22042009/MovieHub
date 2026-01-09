using System.ComponentModel.DataAnnotations;
using MovieHub.Data.Entities.Enums;

namespace MovieHub.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Password { get; set; } = null!;

        [Required]
        public UserRole Role { get; set; } = UserRole.RegisteredUser;

        [Required]
        public UserStatus Status { get; set; } = UserStatus.Active;

        public ICollection<Movie> AddedMovies { get; set; } = new HashSet<Movie>();
    }
}
