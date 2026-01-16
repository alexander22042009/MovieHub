using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MovieHub.Data.Data
{
    public class MovieHubDbContextFactory
        : IDesignTimeDbContextFactory<MovieHubDbContext>
    {
        public MovieHubDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MovieHubDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=MovieHubDb;Trusted_Connection=True;");

            return new MovieHubDbContext(optionsBuilder.Options);
        }
    }
}
