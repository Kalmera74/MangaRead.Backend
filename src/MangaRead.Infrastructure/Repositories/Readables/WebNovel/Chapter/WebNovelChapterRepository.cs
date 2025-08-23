using MangaRead.Application.Repositories.Readables.WebNovel.Chapter;
using MangaRead.Domain.Entities.Readables.WebNovel.Chapter;
using MangaRead.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
namespace MangaRead.Infrastructure.Repositories.Readables.WebNovel.Chapter;
public class WebNovelChapterRepository : Repository<WebNovelChapterEntity>, IWebNovelChapterRepository
{
    public WebNovelChapterRepository(MangaDbContext context) : base(context)
    {
    }

    public async Task<WebNovelChapterEntity?> GetByWebNovelAsync(Guid webNovelId)
    {
        return await _context.Set<WebNovelChapterEntity>().FirstOrDefaultAsync(x => x.WebNovel.Id == webNovelId);
    }

    public async Task<WebNovelChapterEntity?> GetBySlugAsync(string slug)
    {
        return await _context.Set<WebNovelChapterEntity>().FirstOrDefaultAsync(x => x.Slug == slug);
    }

    public async Task<WebNovelChapterEntity?> GetByTitleAsync(string title)
    {
        return await _context.Set<WebNovelChapterEntity>().FirstOrDefaultAsync(x => x.Title == title);
    }

    public async Task<IEnumerable<WebNovelChapterEntity?>> SearchByTitleAsync(string title)
    {
        return await _context.Set<WebNovelChapterEntity>().Where(x => x.Title.Contains(title)).ToListAsync();
    }
}