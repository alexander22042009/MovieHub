using MovieHub.App.DTOs.Users;
using MovieHub.App.Services;
using MovieHub.Data.Models;
using MovieHub.Data.Entities.Enums;


namespace MovieHub.App.DTOs.Users
{
    public class AdminUpdateUserStatusDto
    {
        public int UserId { get; set; }
        public UserStatus NewStatus { get; set; }
    }
}
