using MangaRead.Application.Repositories.Readables.Manga.Chapter;
using MangaRead.Domain.Entities.Readables.Manga.Chapter;
using MangaRead.Infrastructure.DbContexts;
using MangaRead.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaRead.Infrastructure.Repositories.Readables.Manga.Chapter;
public class MangaChapterRepository : Repository<MangaChapterEntity>, IMangaChapterRepository
{
    public MangaChapterRepository(MangaDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<MangaChapterEntity>?> GetAllAsync()
    {
        return await _context.Set<MangaChapterEntity>()
        .Include(x => x.Manga)
        .Include(x => x.Content).ThenInclude(x => x.Item)
        .ToListAsync();
    }

    public override async Task<MangaChapterEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Set<MangaChapterEntity>()
            .Include(x => x.Manga)
            .Include(x => x.Content)
            .Include(x => x.PreviousChapter)
            .Include(x => x.NextChapter)
            .Include(x => x.Content).ThenInclude(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<MangaChapterEntity?> GetBySlugAsync(string slug)
    {
        return await _context.Set<MangaChapterEntity>()
            .Include(x => x.Manga)
            .Include(x => x.Content).ThenInclude(x => x.Item)
            .Include(x => x.PreviousChapter)
            .Include(x => x.NextChapter)
            .FirstOrDefaultAsync(x => x.Slug == slug);
    }

    public async Task<MangaChapterEntity?> GetLatestByMangaAsync(Guid mangaId)
    {
        return await _context.Set<MangaChapterEntity>()
            .Include(x => x.Manga)
            .Include(x => x.Content).ThenInclude(x => x.Item)
            .Include(x => x.PreviousChapter)
            .Include(x => x.NextChapter)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(x => x.Manga.Id == mangaId);
    }

    public override async Task<IEnumerable<MangaChapterEntity>?> GetAllPagedAsync(int pageNumber, int pageSize)
    {
        var items = await _context.Set<MangaChapterEntity>().Skip((pageNumber - 1) * pageSize).Include(x => x.Manga).Take(pageSize).ToListAsync();
        return items;
    }

    async Task<IEnumerable<MangaChapterEntity>?> IMangaChapterRepository.GetByMangaAsync(Guid mangaId)
    {
        return await _context.Set<MangaChapterEntity>().Where(x => x.Manga.Id == mangaId)
            .Include(x => x.Manga)
            .Include(x => x.Content).ThenInclude(x => x.Item)
            .Include(x => x.PreviousChapter)
            .Include(x => x.NextChapter).ToListAsync();
    }
}