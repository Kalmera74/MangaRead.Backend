using MangaRead.Application.Repositories.Status;
using MangaRead.Domain.Entities.Status;
using MangaRead.Infrastructure.DbContexts;
using MangaRead.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaRead.Infrastructure.Repositories.Status;
public class StatusRepository : Repository<StatusEntity>, IStatusRepository
{
    public StatusRepository(MangaDbContext context) : base(context)
    {
    }

    public async Task<StatusEntity?> GetByNameAsync(string name)
    {
        return await _context.Set<StatusEntity>().FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<StatusEntity?> GetBySlugAsync(string slug)
    {
        return await _context.Set<StatusEntity>().FirstOrDefaultAsync(x => x.Slug == slug);
    }

    public async Task<IEnumerable<StatusEntity>> SearchByNameAsync(string name)
    {
        return await _context.Set<StatusEntity>().Where(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync();
    }
}