using MangaRead.Application.Repositories.System.User;
using MangaRead.Domain.Entities.System.User;
using MangaRead.Infrastructure.DbContexts;
using MangaRead.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaRead.Infrastructure.Repositories.System.User;
public class UserRepository : Repository<UserEntity>, IUserRepository
{
    public UserRepository(MangaDbContext context) : base(context)
    {
    }

    public async Task<UserEntity?> GetByEmailAsync(string email)
    {
        return await _context.Set<UserEntity>().FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<ICollection<UserEntity>?> GetByRoles(Guid[] roleIds)
    {
        return await _context.Set<UserEntity>().Where(x => x.Roles.Any(y => roleIds.Contains(y.Id))).ToListAsync();
    }

    public async Task<UserEntity?> GetByUserNameAsync(string userName)
    {
        return await _context.Set<UserEntity>().FirstOrDefaultAsync(x => x.UserName == userName);
    }
}