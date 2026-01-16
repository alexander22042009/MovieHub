using MovieHub.Data.Entities.Enums;

namespace MovieHub.Data.Models
{
    public class CurrentUser
    {
        public int Id { get; set; } = 0;

        public string? Username { get; set; }

        public UserRole Role { get; set; } = UserRole.RegisteredUser;

        public bool IsGuest => Id == 0;
    }
}
