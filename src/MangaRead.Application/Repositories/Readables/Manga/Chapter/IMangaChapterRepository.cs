using MangaRead.Application.Repositories;
using MangaRead.Domain.Entities.Readables.Manga.Chapter;

namespace MangaRead.Application.Repositories.Readables.Manga.Chapter;
public interface IMangaChapterRepository : IRepository<MangaChapterEntity>
{
    public Task<MangaChapterEntity?> GetBySlugAsync(string slug);
    public Task<IEnumerable<MangaChapterEntity>?> GetByMangaAsync(Guid mangaId);
    public Task<MangaChapterEntity?> GetLatestByMangaAsync(Guid mangaId);

}
