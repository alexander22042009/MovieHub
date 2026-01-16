using MovieHub.Data.Entities.Enums;

namespace MovieHub.Data.Models
{
    public class CurrentUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.RegisteredUser;
        public UserStatus Status { get; set; } = UserStatus.Active;

        public bool IsGuest => Id <= 0;
    }
}
