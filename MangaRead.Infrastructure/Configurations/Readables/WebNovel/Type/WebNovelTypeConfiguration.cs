using MangaRead.Domain.Entities.Readables.WebNovel.Type;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaRead.Infrastructure.Configurations.Readables.WebNovel.Type;
public class WebNovelTypeConfiguration : IEntityTypeConfiguration<WebNovelTypeEntity>
{
    public void Configure(EntityTypeBuilder<WebNovelTypeEntity> type)
    {
        type.ToTable("WebNovelType");

        type.HasKey(mt => mt.Id).HasPrefixLength(36);

        type.Property(mt => mt.Id).ValueGeneratedNever();

        type.Property(mt => mt.Name).IsRequired().HasMaxLength(250);
        type.Property(mt => mt.Slug).IsRequired().HasMaxLength(250);

    }
}