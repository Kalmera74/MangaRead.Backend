using MangaRead.Domain.Entities.Readables.WebNovel.Type;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaRead.Infrastructure.Configurations.Readables.WebNovel.Type;
public class WebNovelTypeConfiguration : IEntityTypeConfiguration<WebNovelTypeEntity>
{
    public void Configure(EntityTypeBuilder<WebNovelTypeEntity> type)
    {
        type.ToTable("WebNovelType");

        type.HasKey(wt => wt.Id).HasPrefixLength(36);

        type.Property(wt => wt.Id).ValueGeneratedNever();

        type.Property(wt => wt.Name).IsRequired().HasMaxLength(250);

        type.Property(wt => wt.Slug).IsRequired().HasMaxLength(250);

        type.HasIndex(wt => wt.Slug).IsUnique();

    }
}