using MangaRead.Domain.Entities.Rating;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaRead.Infrastructure.Configurations;
public class RatingConfiguration : IEntityTypeConfiguration<RatingEntity>
{
    public void Configure(EntityTypeBuilder<RatingEntity> rating)
    {
        rating.ToTable("Ratings");

        rating.HasKey(mr => mr.Id).HasPrefixLength(36);

        rating.Property(mr => mr.Id).ValueGeneratedNever();

        rating.Property(mr => mr.StarCount).HasMaxLength(3).IsRequired();

        rating.HasOne(mr => mr.User).WithMany();

    }
}