using MangaRead.Domain.Entities.Readables.Manga.Chapter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaRead.Infrastructure.Configurations.Readables.Manga.Chapter;
public class MangaChapterConfiguration : IEntityTypeConfiguration<MangaChapterEntity>
{
    public void Configure(EntityTypeBuilder<MangaChapterEntity> chapter)
    {
        chapter.ToTable("MangaChapter");

        chapter.HasKey(c => c.Id).HasPrefixLength(36);
        chapter.Property(c => c.Id).ValueGeneratedNever();

        chapter.Property(c => c.Title).HasMaxLength(250).IsRequired();
        chapter.Property(c => c.Slug).HasMaxLength(250).IsRequired();

        chapter.HasOne(c => c.NextChapter);
        chapter.HasOne(c => c.PreviousChapter);

        chapter.HasMany(c => c.Content).WithOne(c => c.Chapter);


        chapter.HasOne(c => c.Manga).WithMany(m => m.Chapters);
    }
}