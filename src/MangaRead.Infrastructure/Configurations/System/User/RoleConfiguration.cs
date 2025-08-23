using MangaRead.Domain.Entities.System.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaRead.Infrastructure.Configurations.System.User;
public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> role)
    {
        role.ToTable("Roles");

        role.HasKey(r => r.Id).HasPrefixLength(36);
        role.Property(r => r.Id).ValueGeneratedOnAdd();

        role.HasMany(r => r.Users).WithMany(u => u.Roles).UsingEntity(ru => ru.ToTable("UserRoles"));

    }
}