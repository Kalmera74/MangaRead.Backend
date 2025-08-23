using MangaRead.Domain.Entities.Status;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaRead.Infrastructure.Configurations;
public class StatusConfiguration : IEntityTypeConfiguration<StatusEntity>
{
    public void Configure(EntityTypeBuilder<StatusEntity> status)
    {
        status.ToTable("Statuses");

        status.HasKey(ms => ms.Id).HasPrefixLength(36);

        status.Property(ms => ms.Id).ValueGeneratedNever();

        status.Property(ms => ms.Name).IsRequired().HasMaxLength(250);

        status.Property(ms => ms.Slug).IsRequired().HasMaxLength(250);
        status.HasIndex(ms => ms.Slug).IsUnique();

    }
}