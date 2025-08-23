using MangaRead.Application.Repositories;
using MangaRead.Domain.Entities.Genre;

namespace MangaRead.Application.Repositories.Genre;
public interface IGenreRepository : IRepository<GenreEntity>
{

    public Task<GenreEntity?> GetBySlugAsync(string slug);
    public Task<GenreEntity?> GetByNameAsync(string name);
    public Task<IList<GenreEntity>?> SearchByNameAsync(string name);
}
