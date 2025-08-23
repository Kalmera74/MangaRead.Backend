using MangaRead.Application.Repositories.System.Image;
using MangaRead.Domain.Entities.System.Image;
using MangaRead.Infrastructure.DbContexts;
using MangaRead.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaRead.Infrastructure.Repositories.System.Image;
public class ImageRepository : Repository<ImageEntity>, IImageRepository
{
    public ImageRepository(MangaDbContext context) : base(context)
    {
    }

    public async Task<ImageEntity?> GetByUrlAsync(string url)
    {
        return await _context.Set<ImageEntity>().FirstOrDefaultAsync(x => x.Url == url);
    }

    public async Task<IEnumerable<ImageEntity>?> SearchByUrlAsync(string PartialUrl)
    {
        return await _context.Set<ImageEntity>().Where(x => x.Url.Contains(PartialUrl)).ToListAsync();
    }
}