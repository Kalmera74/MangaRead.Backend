using MangaRead.Application.Repositories;
using MangaRead.Domain.Entities.Readables.WebNovel.Chapter.Content;

namespace MangaRead.Application.Repositories.Readables.WebNovel.Chapter.Content;
public interface IWebNovelChapterContentRepository : IRepository<WebNovelChapterContentEntity>
{
    public Task<WebNovelChapterContentEntity?> GetByChapterAsync(Guid chapterId);

}
