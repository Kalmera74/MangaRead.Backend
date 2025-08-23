using MangaRead.Domain.Entities.Author;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace MangaRead.Infrastructure.Configurations;
internal class AuthorConfiguration : IEntityTypeConfiguration<AuthorEntity>
{
    public void Configure(EntityTypeBuilder<AuthorEntity> author)
    {
        author.ToTable("Author");

        author.HasKey(a => a.Id).HasPrefixLength(36);
        author.Property(a => a.Id).ValueGeneratedNever();

        author.Property(a => a.Name).IsRequired().HasMaxLength(250);

        author.Property(a => a.Slug).IsRequired().HasMaxLength(250);
        author.HasIndex(a => a.Slug).IsUnique();


    }
}

