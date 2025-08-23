using MangaRead.Domain.Entities.Genre;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaRead.Infrastructure.Configurations;
public class GenreConfiguration : IEntityTypeConfiguration<GenreEntity>
{
    public void Configure(EntityTypeBuilder<GenreEntity> genre)
    {
        genre.ToTable("Genres");

        genre.HasKey(mg => mg.Id).HasPrefixLength(36);

        genre.Property(mg => mg.Id).ValueGeneratedNever();

        genre.Property(mg => mg.Name).IsRequired().HasMaxLength(250);

        genre.Property(mg => mg.Slug).IsRequired().HasMaxLength(250);
        genre.HasIndex(mg => mg.Slug).IsUnique();
    }
}