using MangaRead.Application.Repositories;
using MangaRead.Domain.Entities.System.User;

namespace MangaRead.Application.Repositories.System.User;
public interface IRoleRepository : IRepository<RoleEntity>
{
    public Task<ICollection<RoleEntity>?> GetByUserAsync(Guid userId);
    public Task<RoleEntity?> GetByNameAsync(string name);
}
