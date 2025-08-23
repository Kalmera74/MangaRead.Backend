using MangaRead.Application.Repositories;
using MangaRead.Domain.Entities.Readables.WebNovel.Type;

namespace MangaRead.Application.Repositories.Readables.WebNovel.Type;
public interface IWebNovelTypeRepository : IRepository<WebNovelTypeEntity>
{
    public Task<WebNovelTypeEntity?> GetBySlugAsync(string slug);
    public Task<WebNovelTypeEntity?> GetByNameAsync(string name);
    public Task<IEnumerable<WebNovelTypeEntity>?> SearchByNameAsync(string name);
}
