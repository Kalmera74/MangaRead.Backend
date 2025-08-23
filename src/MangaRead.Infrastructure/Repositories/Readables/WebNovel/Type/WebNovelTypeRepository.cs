using MangaRead.Application.Repositories.Readables.WebNovel.Type;
using MangaRead.Domain.Entities.Readables.WebNovel.Type;
using MangaRead.Infrastructure.DbContexts;
using MangaRead.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaRead.Infrastructure.Repositories.Readables.WebNovel.Type;
public class WebNovelTypeRepository : Repository<WebNovelTypeEntity>, IWebNovelTypeRepository
{
    public WebNovelTypeRepository(MangaDbContext context) : base(context)
    {
    }

    public async Task<WebNovelTypeEntity?> GetByNameAsync(string name)
    {
        return await _context.Set<WebNovelTypeEntity>().FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<WebNovelTypeEntity?> GetBySlugAsync(string slug)
    {
        return await _context.Set<WebNovelTypeEntity>().FirstOrDefaultAsync(x => x.Slug == slug);
    }

    public async Task<IEnumerable<WebNovelTypeEntity>?> SearchByNameAsync(string name)
    {
        return await _context.Set<WebNovelTypeEntity>().Where(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync();
    }
}