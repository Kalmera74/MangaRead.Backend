using MangaRead.Application.Repositories;
using MangaRead.Domain.Entities.Author;

namespace MangaRead.Application.Repositories.Author;

public interface IAuthorRepository : IRepository<AuthorEntity>
{
    public Task<AuthorEntity?> GetBySlugAsync(string slug);
    public Task<AuthorEntity?> GetByNameAsync(string name);
    public Task<IEnumerable<AuthorEntity>?> SearchByNameAsync(string name);

}
