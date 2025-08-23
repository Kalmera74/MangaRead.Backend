using MangaRead.Application.Repositories.Readables.WebNovel.Chapter.Content;
using MangaRead.Domain.Entities.Readables.WebNovel.Chapter.Content;
using MangaRead.Infrastructure.DbContexts;
using MangaRead.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaRead.Infrastructure.Repositories.Readables.WebNovel.Chapter.Content;
public class WebNovelChapterContentRepository : Repository<WebNovelChapterContentEntity>, IWebNovelChapterContentRepository
{
    public WebNovelChapterContentRepository(MangaDbContext context) : base(context)
    {
    }

    public async Task<WebNovelChapterContentEntity?> GetByChapterAsync(Guid chapterId)
    {
        return await _context.Set<WebNovelChapterContentEntity>().FirstOrDefaultAsync(x => x.Chapter.Id == chapterId);
    }
}