using MangaRead.Domain.Entities.Readables.Manga.Chapter.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaRead.Infrastructure.Configurations.Readables.Manga.Chapter.Content;
public class MangaChapterContentConfiguration : IEntityTypeConfiguration<MangaChapterContentEntity>
{
    public void Configure(EntityTypeBuilder<MangaChapterContentEntity> content)
    {
        content.ToTable("MangaChapterContent");

        content.HasKey(cc => cc.Id).HasPrefixLength(36);
        content.Property(cc => cc.Id).ValueGeneratedNever();

        content.HasOne(cc => cc.Item).WithMany();

        content.HasOne(cc => cc.Chapter).WithMany(chapter => chapter.Content);




    }
}