using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieHub.Data.Entities;

namespace MovieHub.Data.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(m => m.Rating)
                .HasColumnType("decimal(3,1)");

            builder.HasIndex(m => new { m.Title, m.ReleaseYear })
                .IsUnique(); 

            builder.HasOne(m => m.Genre)
                .WithMany(g => g.Movies)
                .HasForeignKey(m => m.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.AddedByUser)
                .WithMany(u => u.AddedMovies)
                .HasForeignKey(m => m.AddedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
