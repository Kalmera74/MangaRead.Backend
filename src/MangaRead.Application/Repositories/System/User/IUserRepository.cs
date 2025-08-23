using MangaRead.Application.Repositories;
using MangaRead.Domain.Entities.System.User;

namespace MangaRead.Application.Repositories.System.User;
public interface IUserRepository : IRepository<UserEntity>
{
    public Task<UserEntity?> GetByEmailAsync(string email);
    public Task<UserEntity?> GetByUserNameAsync(string userName);
    public Task<ICollection<UserEntity>?> GetByRoles(Guid[] roleIds);
}
