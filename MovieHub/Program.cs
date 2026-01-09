using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieHub.Data.Data;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");

var options = new DbContextOptionsBuilder<MovieHubDbContext>()
    .UseSqlServer(connectionString)
    .Options;

using var db = new MovieHubDbContext(options);

Console.WriteLine("DB Context created successfully.");
