using MangaRead.Application.Repositories.Readables.Manga.Type;
using MangaRead.Domain.Entities.Readables.Manga.Type;
using MangaRead.Infrastructure.DbContexts;
using MangaRead.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaRead.Infrastructure.Repositories.Readables.Manga.Type;
public class MangaTypeRepository : Repository<MangaTypeEntity>, IMangaTypeRepository
{
    public MangaTypeRepository(MangaDbContext context) : base(context)
    {
    }

    public async Task<MangaTypeEntity?> GetByNameAsync(string name)
    {
        return await _context.Set<MangaTypeEntity>().FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<MangaTypeEntity?> GetBySlugAsync(string slug)
    {
        return await _context.Set<MangaTypeEntity>().FirstOrDefaultAsync(x => x.Slug == slug);
    }

    public async Task<IEnumerable<MangaTypeEntity>?> SearchByNameAsync(string name)
    {
        return await _context.Set<MangaTypeEntity>().Where(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync();
    }
}