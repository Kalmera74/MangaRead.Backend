using MangaRead.Domain.Entities.Readables.WebNovel.Chapter.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaRead.Infrastructure.Configurations.Readables.WebNovel.Chapter.Content;
public class WebNovelChapterContentConfiguration : IEntityTypeConfiguration<WebNovelChapterContentEntity>
{
    public void Configure(EntityTypeBuilder<WebNovelChapterContentEntity> content)
    {
        content.ToTable("WebNovelChapterContent");

        content.HasKey(m => m.Id).HasPrefixLength(36);
        content.Property(m => m.Id).ValueGeneratedNever();

        content.Property(m => m.Title).HasMaxLength(150).IsRequired();
        content.Property(m => m.Body);

        content.Property(m => m.MetaTitle).HasMaxLength(150);
        content.Property(m => m.MetaDescription).HasMaxLength(500);

        content.HasOne(cc => cc.Chapter).WithOne(chapter => chapter.Content);




    }
}