using MangaRead.Application.Repositories.System.User;
using MangaRead.Domain.Entities.System.User;
using MangaRead.Infrastructure.DbContexts;
using MangaRead.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaRead.Infrastructure.Repositories.System.User;
public class RoleRepository : Repository<RoleEntity>, IRoleRepository
{
    public RoleRepository(MangaDbContext context) : base(context)
    {
    }

    public async Task<RoleEntity?> GetByNameAsync(string name)
    {
        return await _context.Set<RoleEntity>().FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<ICollection<RoleEntity>?> GetByUserAsync(Guid userId)
    {
        return await _context.Set<RoleEntity>().Where(x => x.Users.Any(y => y.Id == userId)).ToListAsync();
    }
}