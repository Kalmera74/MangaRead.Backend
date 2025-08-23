using MangaRead.Domain.Entities.Readables.Manga;
using MangaRead.Domain.Entities.Readables.WebNovel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaRead.Infrastructure.Configurations.Readables.Manga;
public class MangaConfiguration : IEntityTypeConfiguration<MangaEntity>
{
    public void Configure(EntityTypeBuilder<MangaEntity> manga)
    {
        manga.ToTable("Manga");

        manga.HasKey(m => m.Id).HasPrefixLength(36);
        manga.Property(m => m.Id).ValueGeneratedNever();

        manga.Property(m => m.Title).HasMaxLength(250).IsRequired();

        manga.Property(m => m.Slug).HasMaxLength(250).IsRequired();
        manga.HasIndex(m => m.Slug).IsUnique();

        manga.Property(m => m.Description).HasMaxLength(5000).IsRequired();

        manga.Property(m => m.MetaTitle).HasMaxLength(150);
        manga.Property(m => m.MetaDescription).HasMaxLength(500);


        manga.HasOne(m => m.CoverImage);

        manga.HasOne(m => m.Status);
        manga.HasOne(m => m.Type);


        manga.HasMany(m => m.Authors).WithMany().UsingEntity(mangaAuthor => mangaAuthor.ToTable("MangaAuthors"));



        manga.HasMany(m => m.Ratings).WithMany().UsingEntity(mangaRating => mangaRating.ToTable("MangaRatings"));



        manga.HasMany(m => m.Genres).WithMany().UsingEntity(mangaGenre => mangaGenre.ToTable("MangaGenres"));



        manga.HasMany(m => m.Chapters).WithOne(chapter => chapter.Manga);



        manga.HasOne(m => m.WebNovel).WithOne(webNovel => webNovel.Manga).HasForeignKey<WebNovelEntity>("MangaId");
    }
}