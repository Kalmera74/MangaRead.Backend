using MangaRead.Application.Repositories;
using MangaRead.Domain.Entities.System.Image;

namespace MangaRead.Application.Repositories.System.Image;

public interface IImageRepository : IRepository<ImageEntity>
{
    public Task<ImageEntity?> GetByUrlAsync(string url);
    public Task<IEnumerable<ImageEntity>?> SearchByUrlAsync(string PartialUrl);
}
