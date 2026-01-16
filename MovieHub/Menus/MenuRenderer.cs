using MovieHub.App.DTOs.Movies;
using MovieHub.App.DTOs.Users;
using MovieHub.App.Services;
using MovieHub.Data.Entities.Enums;
using MovieHub.Data.Models;

namespace MovieHub.App.Menus
{
    public class MenuRenderer
    {
        private readonly AppState _state;
        private readonly AuthService _auth;
        private readonly AdminService _admin;
        private readonly MovieService _movies;

        public MenuRenderer(AppState state, AuthService auth, AdminService admin, MovieService movies)
        {
            _state = state;
            _auth = auth;
            _admin = admin;
            _movies = movies;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                Console.Clear();
                PrintHeader();

                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Continue as Guest");
                Console.WriteLine("0. Exit");
                Console.Write("Choose: ");

                var input = (Console.ReadLine() ?? "").Trim();

                switch (input)
                {
                    case "1":
                        await LoginFlowAsync();
                        break;
                    case "2":
                        await RegisterFlowAsync();
                        break;
                    case "3":
                        _state.CurrentUser = new CurrentUser(); // guest
                        await MainMenuAsync();
                        break;
                    case "0":
                        return;
                    default:
                        Pause("Invalid choice.");
                        break;
                }
            }
        }

        private void PrintHeader()
        {
            if (_state.CurrentUser.IsGuest)
                Console.WriteLine("Logged in as: Guest");
            else
                Console.WriteLine($"Logged in as: {_state.CurrentUser.Username} ({_state.CurrentUser.Role})");

            Console.WriteLine(new string('-', 40));
        }

        private async Task LoginFlowAsync()
        {
            Console.Clear();
            PrintHeader();

            Console.Write("Username: ");
            var username = Console.ReadLine() ?? "";

            Console.Write("Password: ");
            var password = Console.ReadLine() ?? "";

            var (user, message) = await _auth.LoginAsync(new LoginDto { Username = username, Password = password });

            if (user == null)
            {
                Pause(message);
                return;
            }

            _state.CurrentUser = user;
            Pause(message);

            await MainMenuAsync();
        }

        private async Task RegisterFlowAsync()
        {
            Console.Clear();
            PrintHeader();

            Console.Write("Username: ");
            var username = Console.ReadLine() ?? "";

            Console.Write("Password: ");
            var password = Console.ReadLine() ?? "";

            Console.Write("Confirm password: ");
            var confirm = Console.ReadLine() ?? "";

            var (ok, message) = await _auth.RegisterAsync(new RegisterUserDto
            {
                Username = username,
                Password = password,
                ConfirmPassword = confirm
            });

            Pause(message);
        }

        private async Task MainMenuAsync()
        {
            while (true)
            {
                Console.Clear();
                PrintHeader();

                // -------- GUEST --------
                if (_state.CurrentUser.IsGuest)
                {
                    Console.WriteLine("1. List movies");
                    Console.WriteLine("2. Search movies");
                    Console.WriteLine("0. Logout");
                    Console.Write("Choose: ");

                    var input = (Console.ReadLine() ?? "").Trim();

                    switch (input)
                    {
                        case "1":
                            await MoviesListAsync();
                            break;
                        case "2":
                            await MoviesSearchAsync();
                            break;
                        case "0":
                            _state.CurrentUser = new CurrentUser();
                            return;
                        default:
                            Pause("Invalid choice.");
                            break;
                    }

                    continue;
                }

                // -------- REGISTERED USER --------
                if (_state.CurrentUser.Role == UserRole.RegisteredUser)
                {
                    Console.WriteLine("1. List movies");
                    Console.WriteLine("2. Search movies");
                    Console.WriteLine("3. Add movie");
                    Console.WriteLine("0. Logout");
                    Console.Write("Choose: ");

                    var input = (Console.ReadLine() ?? "").Trim();

                    switch (input)
                    {
                        case "1":
                            await MoviesListAsync();
                            break;
                        case "2":
                            await MoviesSearchAsync();
                            break;
                        case "3":
                            await MoviesAddAsync();
                            break;
                        case "0":
                            _state.CurrentUser = new CurrentUser();
                            return;
                        default:
                            Pause("Invalid choice.");
                            break;
                    }

                    continue;
                }

                // -------- ADMIN --------
                if (_state.CurrentUser.Role == UserRole.Administrator)
                {
                    Console.WriteLine("1. List users");
                    Console.WriteLine("2. Block user");
                    Console.WriteLine("3. Unblock user");
                    Console.WriteLine("4. Delete user");
                    Console.WriteLine("5. List movies");
                    Console.WriteLine("6. Add movie");
                    Console.WriteLine("0. Logout");
                    Console.Write("Choose: ");

                    var input = (Console.ReadLine() ?? "").Trim();

                    switch (input)
                    {
                        case "1":
                            await AdminListUsersAsync();
                            break;
                        case "2":
                            await AdminBlockUnblockAsync(true);
                            break;
                        case "3":
                            await AdminBlockUnblockAsync(false);
                            break;
                        case "4":
                            await AdminDeleteUserAsync();
                            break;
                        case "5":
                            await MoviesListAsync();
                            break;
                        case "6":
                            await MoviesAddAsync();
                            break;
                        case "0":
                            _state.CurrentUser = new CurrentUser();
                            return;
                        default:
                            Pause("Invalid choice.");
                            break;
                    }

                    continue;
                }

                // fallback
                _state.CurrentUser = new CurrentUser();
                Pause("Unknown role. Logged out.");
                return;
            }
        }

        // ---------------- ADMIN ----------------

        private async Task AdminListUsersAsync()
        {
            Console.Clear();
            PrintHeader();

            var users = await _admin.GetAllUsersAsync();

            Console.WriteLine("ID | Username | Role | Status");
            Console.WriteLine(new string('-', 60));

            foreach (var u in users)
                Console.WriteLine($"{u.Id} | {u.Username} | {u.Role} | {u.Status}");

            Pause("End of list.");
        }

        private async Task AdminBlockUnblockAsync(bool block)
        {
            Console.Clear();
            PrintHeader();

            Console.Write("Enter User Id: ");
            if (!int.TryParse(Console.ReadLine(), out var userId))
            {
                Pause("Invalid number.");
                return;
            }

            var dto = new AdminUpdateUserStatusDto
            {
                UserId = userId,
                NewStatus = block ? UserStatus.Blocked : UserStatus.Active
            };

            var (_, message) = await _admin.UpdateUserStatusAsync(dto);
            Pause(message);
        }

        private async Task AdminDeleteUserAsync()
        {
            Console.Clear();
            PrintHeader();

            Console.Write("Enter User Id to delete: ");
            if (!int.TryParse(Console.ReadLine(), out var userId))
            {
                Pause("Invalid number.");
                return;
            }

            var (_, message) = await _admin.DeleteUserAsync(new AdminDeleteUserDto { UserId = userId });
            Pause(message);
        }

        // ---------------- MOVIES ----------------

        private async Task MoviesListAsync()
        {
            Console.Clear();
            PrintHeader();

            var movies = await _movies.GetAllMoviesAsync();

            Console.WriteLine("ID | Title | ReleaseYear | Genre | Rating | AddedBy");
            Console.WriteLine(new string('-', 90));

            foreach (var m in movies)
                Console.WriteLine($"{m.Id} | {m.Title} | {m.ReleaseYear} | {m.Genre} | {m.Rating} | {m.AddedBy}");

            Pause("End of list.");
        }

        private async Task MoviesSearchAsync()
        {
            Console.Clear();
            PrintHeader();

            Console.Write("Search text: ");
            var text = (Console.ReadLine() ?? "").Trim();

            var movies = await _movies.SearchMoviesAsync(text);

            Console.WriteLine("ID | Title | ReleaseYear | Genre | Rating | AddedBy");
            Console.WriteLine(new string('-', 90));

            foreach (var m in movies)
                Console.WriteLine($"{m.Id} | {m.Title} | {m.ReleaseYear} | {m.Genre} | {m.Rating} | {m.AddedBy}");

            Pause("End of results.");
        }

        private async Task MoviesAddAsync()
        {
            Console.Clear();
            PrintHeader();

            if (_state.CurrentUser.IsGuest)
            {
                Pause("Guests cannot add movies. Please login/register.");
                return;
            }

            var genres = await _movies.GetGenresAsync();
            if (genres.Count == 0)
            {
                Pause("No genres found. Add/seed genres first.");
                return;
            }

            Console.Write("Title: ");
            var title = Console.ReadLine() ?? "";

            Console.Write("Release year: ");
            int.TryParse(Console.ReadLine(), out var releaseYear);

            Console.Write("Rating (0-10): ");
            decimal.TryParse(Console.ReadLine(), out var rating);

            Console.WriteLine();
            Console.WriteLine("Genres:");
            foreach (var g in genres)
                Console.WriteLine($"{g.id}. {g.name}");

            Console.Write("Choose Genre Id: ");
            int.TryParse(Console.ReadLine(), out var genreId);

            var dto = new AddMovieDto
            {
                Title = title,
                Year = releaseYear,
                Rating = rating,
                GenreId = genreId
            };

            var (ok, message) = await _movies.AddMovieAsync(dto, _state.CurrentUser.Id);
            Pause(message);
        }

        private static void Pause(string message)
        {
            Console.WriteLine();
            Console.WriteLine(message);
            Console.WriteLine("Press Enter...");
            Console.ReadLine();
        }
    }
}
