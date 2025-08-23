using MangaRead.Application.Repositories.Readables.Manga.Chapter.Content;
using MangaRead.Domain.Entities.Readables.Manga.Chapter.Content;
using MangaRead.Infrastructure.DbContexts;
using MangaRead.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaRead.Infrastructure.Repositories.Readables.Manga.Chapter.Content;
public class MangaChapterContentRepository : Repository<MangaChapterContentEntity>, IMangaChapterContentRepository
{
    public MangaChapterContentRepository(MangaDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<MangaChapterContentEntity>?> GetAllAsync()
    {
        return await _context.Set<MangaChapterContentEntity>()
        .Include(x => x.Item)
        .ToListAsync();
    }
    public override async Task<MangaChapterContentEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Set<MangaChapterContentEntity>()
        .Include(x => x.Item)
        .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<MangaChapterContentEntity?> GetByChapterAsync(Guid chapterId)
    {
        return await _context.Set<MangaChapterContentEntity>()
        .Include(x => x.Item)
        .FirstOrDefaultAsync(x => x.Chapter.Id == chapterId);
    }
}