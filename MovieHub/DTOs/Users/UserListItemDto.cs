using MovieHub.App.DTOs.Users;
using MovieHub.App.Services;
using MovieHub.Data.Models;
using MovieHub.Data.Entities.Enums;

namespace MovieHub.App.DTOs.Users
{
    public class UserListItemDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public UserRole Role { get; set; }
        public UserStatus Status { get; set; }
    }
}
