using Microsoft.EntityFrameworkCore;
using MovieHub.App.DTOs.Users;
using MovieHub.Data.Data;
using MovieHub.Data.Entities;
using MovieHub.Data.Entities.Enums;
using MovieHub.Data.Models;

namespace MovieHub.App.Services
{
    public class AuthService
    {
        private readonly MovieHubDbContext _db;

        public AuthService(MovieHubDbContext db)
        {
            _db = db;
        }

        public async Task<(bool ok, string message)> RegisterAsync(RegisterUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) ||
                string.IsNullOrWhiteSpace(dto.Password) ||
                string.IsNullOrWhiteSpace(dto.ConfirmPassword))
            {
                return (false, "Username and password are required.");
            }

            if (dto.Password != dto.ConfirmPassword)
                return (false, "Passwords do not match.");

            var username = dto.Username.Trim();

            var exists = await _db.Users.AnyAsync(u => u.Username == username);
            if (exists)
                return (false, "Username already exists.");

            var user = new User
            {
                Username = username,
                Password = dto.Password, // НЕ се хешира по условие
                Role = UserRole.RegisteredUser,
                Status = UserStatus.Active
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return (true, "Registration successful.");
        }

        public async Task<(CurrentUser? user, string message)> LoginAsync(LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return (null, "Username and password are required.");

            var username = dto.Username.Trim();

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return (null, "Invalid username or password.");

            if (user.Password != dto.Password) // НЕ се хешира по условие
                return (null, "Invalid username or password.");

            if (user.Status == UserStatus.Blocked)
                return (null, "Your account is blocked.");

            var currentUser = new CurrentUser
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                Status = user.Status
            };

            return (currentUser, "Login successful.");
        }
    }
}
