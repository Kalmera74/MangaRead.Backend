using MangaRead.Application.Repositories;
using MangaRead.Domain.Entities.Readables.WebNovel.Chapter;

namespace MangaRead.Application.Repositories.Readables.WebNovel.Chapter;

public interface IWebNovelChapterRepository : IRepository<WebNovelChapterEntity>
{
    public Task<WebNovelChapterEntity?> GetBySlugAsync(string slug);
    public Task<WebNovelChapterEntity?> GetByTitleAsync(string title);
    public Task<IEnumerable<WebNovelChapterEntity?>> SearchByTitleAsync(string title);
    public Task<WebNovelChapterEntity?> GetByWebNovelAsync(Guid webNovelId);

}
