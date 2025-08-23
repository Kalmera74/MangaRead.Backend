using MangaRead.Application.Repositories;
using MangaRead.Domain.Entities.Readables.Manga;

namespace MangaRead.Application.Repositories.Readables.Manga;
public interface IMangaRepository : IRepository<MangaEntity>
{
    public Task<MangaEntity?> GetByTitleAsync(string title);
    public Task<IEnumerable<MangaEntity>?> GetSimilarToAsync(Guid mangaId, int take = 10);
    public Task<IEnumerable<MangaEntity>?> SearchByTitleAsync(string title, int take = 5);

    public Task<MangaEntity?> GetBySlugAsync(string slug);

    public Task<IEnumerable<MangaEntity>?> GetByCreatedAtAsync(DateTime startDate, DateTime endDate, int take = 10);
    public Task<IEnumerable<MangaEntity>?> GetByUpdatedAtAsync(DateTime startDate, DateTime endDate, int take = 10);

    public Task<IEnumerable<MangaEntity>?> GetByViewsAsync(int take = 10);

    public Task<IEnumerable<MangaEntity>?> GetRandomAsync(int take = 1);

    public Task<MangaEntity?> GetByRatingAsync(Guid ratingId);
    public Task<IEnumerable<MangaEntity>?> GetByRatingsAsync(int min, int max);

    public Task<IEnumerable<MangaEntity>?> GetByRandomAsync(int take = 10);

    public Task<MangaEntity?> GetByWebNovelAsync(Guid webNovelId);

    public Task<IEnumerable<MangaEntity>?> GetByTypeAsync(Guid typeId);
    public Task<IEnumerable<MangaEntity>?> GetByTypesAsync(Guid[] typeIds);

    public Task<IEnumerable<MangaEntity>?> GetByGenreAsync(Guid genreId);
    public Task<IEnumerable<MangaEntity>?> GetByGenresAsync(Guid[] genreIds);

    public Task<IEnumerable<MangaEntity>?> GetByStatusAsync(Guid statusId);
    public Task<IEnumerable<MangaEntity>?> GetByStatusesAsync(Guid[] statusIds);

    public Task<IEnumerable<MangaEntity>?> GetByAuthorAsync(Guid authorId);
    public Task<IEnumerable<MangaEntity>?> GetByAuthorsAsync(Guid[] authorIds);

    public Task<IEnumerable<MangaEntity>?> GetFilteredMangasAsync(Guid[]? typeIds, Guid[]? genreIds, Guid[]? statusIds);
    public Task<IEnumerable<MangaEntity>?> GetFilteredMangasPagedAsync(Guid[]? typeIds, Guid[]? genreIds, Guid[]? statusIds, int skip, int take);


}
