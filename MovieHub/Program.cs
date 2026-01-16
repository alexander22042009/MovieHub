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
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<MovieHubDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using var db = new MovieHubDbContext(options);

            var state = new AppState();
            var authService = new AuthService(db);
            var adminService = new AdminService(db);
            var movieService = new MovieService(db);
            var actorService = new ActorService(db);

            var menu = new MenuRenderer(state, authService, adminService, movieService, actorService); 

            await menu.RunAsync();
        }
    }
}
