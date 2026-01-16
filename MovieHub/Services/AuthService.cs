using Microsoft.EntityFrameworkCore;
using MovieHub.App.DTOs.Users;
using MovieHub.Data.Data;
using MovieHub.Data.Models;
using MovieHub.Data.Entities;
using MovieHub.Data.Entities.Enums;
using System.Security.Cryptography;
using System.Text;

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
            if (string.IsNullOrWhiteSpace(dto.Username) || dto.Username.Length < 3)
                return (false, "Username must be at least 3 characters.");

            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 4)
                return (false, "Password must be at least 4 characters.");

            if (dto.Password != dto.ConfirmPassword)
                return (false, "Passwords do not match.");

            var exists = await _db.Users.AnyAsync(u => u.Username == dto.Username);
            if (exists)
                return (false, "Username already exists.");

            var user = new User
            {
                Username = dto.Username.Trim(),
                Password = dto.Password,
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

            var passwordHash = dto.Password;

            var user = await _db.Users
                .Where(u => u.Username == dto.Username)
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Password,
                    u.Role,
                    u.Status
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return (null, "Invalid username or password.");

            if (user.Status == UserStatus.Blocked)
                return (null, "Your account is blocked. Contact administrator.");

            if (user.Password != passwordHash)
                return (null, "Invalid username or password.");

            return (new CurrentUser
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            }, "Login successful.");
        }
    }
}
