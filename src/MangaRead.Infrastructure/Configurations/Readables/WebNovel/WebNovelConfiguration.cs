using MangaRead.Domain.Entities.Readables.Manga;
using MangaRead.Domain.Entities.Readables.WebNovel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaRead.Infrastructure.Configurations.Readables.WebNovel;
public class WebNovelConfiguration : IEntityTypeConfiguration<WebNovelEntity>
{
    public void Configure(EntityTypeBuilder<WebNovelEntity> webNovel)
    {
        webNovel.ToTable("WebNovel");

        webNovel.HasKey(wn => wn.Id).HasPrefixLength(36);
        webNovel.Property(wn => wn.Id).ValueGeneratedNever();

        webNovel.Property(wn => wn.Title).HasMaxLength(250).IsRequired();

        webNovel.Property(wn => wn.Slug).HasMaxLength(250).IsRequired();
        webNovel.HasIndex(wn => wn.Slug).IsUnique();

        webNovel.Property(wn => wn.Description).IsRequired();

        webNovel.Property(wn => wn.MetaTitle).HasMaxLength(150);
        webNovel.Property(wn => wn.MetaDescription).HasMaxLength(500);


        webNovel.HasOne(wn => wn.CoverImage);
        webNovel.HasOne(wn => wn.Status);
        webNovel.HasOne(wn => wn.Type);

        webNovel.HasMany(wn => wn.Authors).WithMany().UsingEntity(webNovelAuthor => webNovelAuthor.ToTable("WebNovelAuthors"));

        webNovel.HasMany(wn => wn.Ratings).WithMany().UsingEntity(webNovelRating => webNovelRating.ToTable("WebNovelRatings"));

        webNovel.HasMany(wn => wn.Genres).WithMany().UsingEntity(webNovelGenre => webNovelGenre.ToTable("WebNovelGenres"));

        webNovel.HasMany(wn => wn.Chapters).WithOne(chapter => chapter.WebNovel);

        webNovel.HasOne(wn => wn.Manga).WithOne(manga => manga.WebNovel).HasForeignKey<MangaEntity>("WebNovelId");
    }
}