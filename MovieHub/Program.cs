using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieHub.App.Menus;
using MovieHub.App.Services;
using MovieHub.Data.Data;
using MovieHub.Data.Models;

namespace MovieHub.App
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Configuration
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            // DbContext
            var options = new DbContextOptionsBuilder<MovieHubDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using var db = new MovieHubDbContext(options);

            // App state + services
            var state = new AppState();
            var authService = new AuthService(db);
            var adminService = new AdminService(db);
            var movieService = new MovieService(db); // ✅ NEW

            // Menu
            var menu = new MenuRenderer(state, authService, adminService, movieService); // ✅ NEW

            // Start app
            await menu.RunAsync();
        }
    }
}
