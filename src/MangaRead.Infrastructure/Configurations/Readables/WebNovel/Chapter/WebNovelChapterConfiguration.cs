using MangaRead.Domain.Entities.Readables.WebNovel.Chapter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaRead.Infrastructure.Configurations.Readables.WebNovel.Chapter;
public class WebNovelChapterConfiguration : IEntityTypeConfiguration<WebNovelChapterEntity>
{
    public void Configure(EntityTypeBuilder<WebNovelChapterEntity> chapter)
    {
        chapter.ToTable("WebNovelChapter");

        chapter.HasKey(c => c.Id).HasPrefixLength(36);
        chapter.Property(c => c.Id).ValueGeneratedNever();

        chapter.Property(c => c.Title).HasMaxLength(250).IsRequired();
        chapter.Property(c => c.Slug).HasMaxLength(250).IsRequired();


        chapter.HasOne(c => c.NextChapter);
        chapter.HasOne(c => c.PreviousChapter);

        chapter.HasOne(c => c.Content).WithOne(c => c.Chapter).HasForeignKey<WebNovelChapterEntity>("ContentId");

        chapter.HasOne(c => c.WebNovel).WithMany(m => m.Chapters);


    }
}