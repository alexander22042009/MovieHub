    using MovieHub.App.DTOs.Users;
    using MovieHub.Data.Entities.Enums;
    using Microsoft.EntityFrameworkCore;
    using MovieHub.Data.Data;

namespace MovieHub.App.Services
    {
        public class AdminService
        {
            private readonly MovieHubDbContext _db;

            public AdminService(MovieHubDbContext db)
            {
                _db = db;
            }

            // ✅ Admin-only: list users (DTO)
            public async Task<List<UserListItemDto>> GetAllUsersAsync()
            {
                return await _db.Users
                    .OrderBy(u => u.Id)
                    .Select(u => new UserListItemDto
                    {
                        Id = u.Id,
                        Username = u.Username,
                        Role = u.Role,
                        Status = u.Status
                    })
                    .ToListAsync();
            }

            // ✅ Admin-only: block/unblock
            public async Task<(bool ok, string message)> UpdateUserStatusAsync(AdminUpdateUserStatusDto dto)
            {
                if (dto.UserId <= 0)
                    return (false, "Invalid user id.");

                var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == dto.UserId);
                if (user == null)
                    return (false, "User not found.");

                if (user.Role == UserRole.Administrator)
                    return (false, "You cannot change status of an Administrator.");

                user.Status = dto.NewStatus;
                await _db.SaveChangesAsync();

                return (true, $"User '{user.Username}' status set to {dto.NewStatus}.");
            }

            // ✅ Admin-only: delete user
            public async Task<(bool ok, string message)> DeleteUserAsync(AdminDeleteUserDto dto)
            {
                if (dto.UserId <= 0)
                    return (false, "Invalid user id.");

                var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == dto.UserId);
                if (user == null)
                    return (false, "User not found.");

                if (user.Role == UserRole.Administrator)
                    return (false, "You cannot delete an Administrator.");

                // Ако имаш Restrict към Movies (AddedByUser) -> първо трябва да се изтрият филмите на този user
                // (ще го решим по-късно при MovieService; за момента даваме ясно съобщение).
                var hasMovies = await _db.Movies.AnyAsync(m => m.AddedByUserId == dto.UserId);
                if (hasMovies)
                    return (false, "Cannot delete user because they have added movies. Delete or reassign their movies first.");

                _db.Users.Remove(user);
                await _db.SaveChangesAsync();

                return (true, $"User '{user.Username}' deleted.");
            }
        }
    }
