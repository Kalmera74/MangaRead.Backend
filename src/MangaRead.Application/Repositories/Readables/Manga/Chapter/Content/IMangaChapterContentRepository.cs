using MangaRead.Application.Repositories;
using MangaRead.Domain.Entities.Readables.Manga.Chapter.Content;

namespace MangaRead.Application.Repositories.Readables.Manga.Chapter.Content;
public interface IMangaChapterContentRepository : IRepository<MangaChapterContentEntity>
{
    public Task<MangaChapterContentEntity?> GetByChapterAsync(Guid chapterId);
}

