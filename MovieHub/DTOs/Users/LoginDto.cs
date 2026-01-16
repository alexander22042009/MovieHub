using MovieHub.App.DTOs.Users;
using MovieHub.App.Services;
using MovieHub.Data.Models;
using MovieHub.Data.Entities.Enums;

namespace MovieHub.App.DTOs.Users
{
    public class LoginDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
