using MangaRead.Application.Repositories;
using MangaRead.Domain.Entities.Status;

namespace MangaRead.Application.Repositories.Status;
public interface IStatusRepository : IRepository<StatusEntity>
{
    public Task<StatusEntity?> GetByNameAsync(string name);

    public Task<StatusEntity?> GetBySlugAsync(string slug);
    public Task<IEnumerable<StatusEntity>> SearchByNameAsync(string name);

}
