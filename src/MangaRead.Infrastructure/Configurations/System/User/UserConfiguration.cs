using MangaRead.Domain.Entities.System.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaRead.Infrastructure.Configurations.System.User;
public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> user)
    {
        user.ToTable("Users");

        user.HasKey(u => u.Id).HasPrefixLength(36);
        user.Property(u => u.Id).ValueGeneratedOnAdd();

        user.Property(u => u.Email).HasMaxLength(250).IsRequired();
        user.Property(u => u.UserName).HasMaxLength(250).IsRequired();
        user.Property(u => u.Password).HasMaxLength(250).IsRequired();

        user.HasMany(u => u.Roles).WithMany(r => r.Users).UsingEntity(userRole => userRole.ToTable("UserRoles"));

        user.HasMany(u => u.BookMarkedMangas).WithMany().UsingEntity(userBookmarkedManga => userBookmarkedManga.ToTable("UserBookmarkedMangas"));

        user.HasMany(u => u.BookMarkedWebNovels).WithMany().UsingEntity(userBookmarkedWebNovel => userBookmarkedWebNovel.ToTable("UserBookmarkedWebNovels"));

    }
}