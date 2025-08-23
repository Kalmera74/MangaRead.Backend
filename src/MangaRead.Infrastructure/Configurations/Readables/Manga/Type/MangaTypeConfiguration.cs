using MangaRead.Domain.Entities.Readables.Manga.Type;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaRead.Infrastructure.Configurations.Readables.Manga.Type;
public class MangaTypeConfiguration : IEntityTypeConfiguration<MangaTypeEntity>
{
    public void Configure(EntityTypeBuilder<MangaTypeEntity> type)
    {
        type.ToTable("MangaType");

        type.HasKey(mt => mt.Id).HasPrefixLength(36);

        type.Property(mt => mt.Id).ValueGeneratedNever();

        type.Property(mt => mt.Name).IsRequired().HasMaxLength(250);

        type.Property(mt => mt.Slug).IsRequired().HasMaxLength(250);

        type.HasIndex(mt => mt.Slug).IsUnique();

    }
}