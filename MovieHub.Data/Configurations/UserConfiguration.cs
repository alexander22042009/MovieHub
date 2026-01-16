using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieHub.Data.Entities;
using MovieHub.Data.Entities.Enums;


namespace MovieHub.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.Username)
                .IsUnique();

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.Role)
                .IsRequired();

            builder.Property(u => u.Status)
                .IsRequired();


            builder.HasMany(u => u.AddedMovies)
                .WithOne(m => m.AddedByUser)
                .HasForeignKey(m => m.AddedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
