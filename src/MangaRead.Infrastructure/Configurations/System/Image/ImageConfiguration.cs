using MangaRead.Domain.Entities.System.Image;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaRead.Infrastructure.Configurations.System.Image;
public class ImageConfiguration : IEntityTypeConfiguration<ImageEntity>
{
    public void Configure(EntityTypeBuilder<ImageEntity> image)
    {
        image.ToTable("Image");

        image.HasKey(i => i.Id).HasPrefixLength(36);
        image.Property(i => i.Id).ValueGeneratedNever();

        image.Property(i => i.Url).HasMaxLength(2048).IsRequired();
    }
}