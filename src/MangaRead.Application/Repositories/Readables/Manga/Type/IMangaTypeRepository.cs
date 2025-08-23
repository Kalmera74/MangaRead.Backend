using MangaRead.Application.Repositories;
using MangaRead.Domain.Entities.Readables.Manga.Type;

namespace MangaRead.Application.Repositories.Readables.Manga.Type;
public interface IMangaTypeRepository : IRepository<MangaTypeEntity>
{
    public Task<MangaTypeEntity?> GetBySlugAsync(string slug);
    public Task<MangaTypeEntity?> GetByNameAsync(string name);
    public Task<IEnumerable<MangaTypeEntity>?> SearchByNameAsync(string name);
}
